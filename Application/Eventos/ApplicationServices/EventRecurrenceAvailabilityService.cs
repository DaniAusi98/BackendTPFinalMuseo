using Application.ActividadMuseo.Repositories;
using Application.Availability;
using Application.Availability.ApplicationServices;
using Application.Availability.Factories;
using Application.Availability.Models;
using Application.Eventos.DataTransferObjets;
using Application.MuseumResources.Repositories;
using Domain.Common.ValueObjets;
using Domain.Eventos.Entities;
using Microsoft.Extensions.Logging;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.Eventos.Enums.Enums;

namespace Application.Eventos.ApplicationServices
{
    public class EventRecurrenceAvailabilityService
        : IEventRecurrenceAvailabilityService
    {
        private readonly IRepositorioActividadMuseo _repositorioActividadMuseo;
        private readonly IRepositorioCalendarioMuseo _repositorioCalendario;
        private readonly IRepositorioSala _repositorioSalaMuseo;
        private readonly AvailabilityEngine _engine;
        private readonly ActivityAvailabilityFactory _availabilityFactory;
        private readonly IRecurrenceEvaluator _recurrenceEvaluator;
        private readonly ILogger<EventRecurrenceAvailabilityService> _logger;

        public EventRecurrenceAvailabilityService(
            IRepositorioActividadMuseo repositorioActividadMuseo,
            IRepositorioCalendarioMuseo repositorioCalendario,
            IRepositorioSala repositorioSalaMuseo,
            AvailabilityEngine engine,
            ActivityAvailabilityFactory availabilityFactory,
            IRecurrenceEvaluator recurrenceEvaluator,
            ILogger<EventRecurrenceAvailabilityService> logger)
        {
            _repositorioActividadMuseo = repositorioActividadMuseo;
            _repositorioCalendario = repositorioCalendario;
            _repositorioSalaMuseo = repositorioSalaMuseo;
            _engine = engine;
            _availabilityFactory = availabilityFactory;
            _recurrenceEvaluator = recurrenceEvaluator;
            _logger = logger;
        }

        public async Task<ValidarRecurrenciaEventoDto> ValidarAsync(
            DateTime inicio,
            DateTime fin,
            List<string> salasIds,
            string rrule)
        {
            // Acá después resolvemos windowStart/windowEnd.
            // Por ahora usamos inicio/fin.
            var duracionMinutos = (int)(fin - inicio).TotalMinutes;

            var windowStart = inicio;

            var windowEnd = _recurrenceEvaluator.GetWindowEnd(
                rrule,
                inicio,
                duracionMinutos);

            // 1. Calendario activo
            var calendario =
                await _repositorioCalendario.ObtenerCalendarioActivoAsync();

            if (calendario == null)
            {
                throw new InvalidOperationException(
                    "No hay calendario activo del museo.");
            }

            // 2. Obtener actividades existentes
            var actividadesEnRango =
                await _repositorioActividadMuseo.FindAllAsync(
                    windowStart,
                    windowEnd);

            // 3. Prepararlas exactamente igual que en el Producer
            var existingActivities =
                _availabilityFactory.Create(
                    [.. actividadesEnRango],
                    windowStart,
                    windowEnd);

            // 4. Buscar las salas una sola vez
            var salas =
                await _repositorioSalaMuseo
                    .ObtenerSalasporIdsAsync(salasIds);

            // 5. Duración del evento
            var duracionEventominutos =
                (int)(fin - inicio).TotalMinutes;

            // 6. Expandir recurrencia
            var timeSlots =
                _recurrenceEvaluator.ExpandRule(
                    rrule,
                    inicio,
                    duracionMinutos,
                    windowStart,
                    windowEnd);

            // 7. Crear candidatos
            var candidatos = new List<CandidateEntry>();

            foreach (var timeSlot in timeSlots)
            {
                var eventoFicticio = new Evento(
                    nombreyApellidoSolicitante: "Disponibilidad",
                    telefonoSolicitante:
                        new Telefono("3511234567"),
                    emailSolicitante:
                        new Email("disponibilidad@museo.com"),
                    institucion: "Museo",
                    tipoEvento: TipoEvento.Conferencia,
                    tituloEvento: "Evento ficticio",
                    descripcionEvento: "",
                    fundamentacionEvento: "",
                    tipoPublico:
                        new List<TipoPublico>
                        {
                            TipoPublico.General
                        },
                    concurrenciaEstimada: 1,
                    horario: timeSlot,
                    salas: salas,
                    requiereDifusion: false,
                    solicitaFlyer: false,
                    recurrenceRule: null);

                candidatos.Add(
                    new CandidateEntry(
                        Guid.NewGuid(),
                        eventoFicticio,
                        new List<TimeSlot>
                        {
                            timeSlot
                        }.AsReadOnly(),
                        null,
                        "EventoRecurrence"));
            }

            // 8. Contexto del Engine
            var contexto = new AvailabilityContext
            {
                Start = windowStart,
                End = windowEnd,

                ExistingActivities =
                    existingActivities,

                Metadata = new Dictionary<string, object>
                {
                    {
                        "tipoActividad",
                        TipoActividad.Evento
                    },
                    {
                        "salasIds",
                        salasIds
                    }
                }
            };

            // 9. Ejecutar Engine
            var resultados =
                await _engine.CheckManyAsync(
                    contexto,
                    candidatos);

            // 10. Obtener solamente los slots que fallaron
            var errores = new List<SlotErrorDto>();

            foreach (var candidato in candidatos)
            {
                if (!resultados.TryGetValue(
                        candidato.Id,
                        out var resultado))
                {
                    continue;
                }

                if (resultado.IsOk)
                {
                    continue;
                }

                var slot = candidato.TimeSlots.First();

                errores.Add(
                    new SlotErrorDto
                    {
                        Inicio = slot.Inicio,
                        Fin = slot.Fin,
                        Message = resultado.Message
                    });
            }

            // 11. Respuesta
            return new ValidarRecurrenciaEventoDto
            {
                IsValid = errores.Count == 0,
                Errores = errores
            };
        }
    }
}