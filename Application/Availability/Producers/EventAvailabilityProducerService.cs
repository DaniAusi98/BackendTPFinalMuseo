
using Application.ActividadMuseo.Repositories;
using Application.Availability.Factories;
using Application.Availability.Models;
using Application.ConfiguracionSistema.Repositories;
using Application.Eventos.DataTransferObjets;
using Application.MuseumResources.Repositories;
using Domain.Common.ValueObjets;
using Domain.Eventos.Entities;
using Microsoft.Extensions.Logging;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.Eventos.Enums.Enums;
using ActividadMuseoEntity = Domain.ActividadMuseo.Entities.ActividadMuseo;

namespace Application.Availability.Producers
{
    public class EventAvailabilityProducerService
    {
        private readonly IRepositorioActividadMuseo _repositorioActividadMuseo;
        private readonly IRepositorioCalendarioMuseo _repositorioCalendario;
        private readonly IRepositorioSala _repositorioSalaMuseo;
        private readonly AvailabilityEngine _engine;
        private readonly ActivityAvailabilityFactory _availabilityFactory;
        private readonly ILogger<EventAvailabilityProducerService> _logger;

        public EventAvailabilityProducerService(
            IRepositorioActividadMuseo repositorioActividadMuseo,
            IRepositorioCalendarioMuseo repositorioCalendario,
            IRepositorioSala repositorioSalaMuseo,
            AvailabilityEngine engine,
            ActivityAvailabilityFactory availabilityFactory,
            ILogger<EventAvailabilityProducerService> logger)
        {
            _repositorioActividadMuseo = repositorioActividadMuseo
                ?? throw new ArgumentNullException(nameof(repositorioActividadMuseo));

            _repositorioCalendario = repositorioCalendario
                ?? throw new ArgumentNullException(nameof(repositorioCalendario));

            _repositorioSalaMuseo = repositorioSalaMuseo
                ?? throw new ArgumentNullException(nameof(repositorioSalaMuseo));

            _engine = engine
                ?? throw new ArgumentNullException(nameof(engine));

            _availabilityFactory = availabilityFactory
                ?? throw new ArgumentNullException(nameof(availabilityFactory));

            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<EventoDisponibilidadDto> GetEventAvailabilityAsync(
            DateTime desde,
            DateTime hasta,
            List<string> salasIds)
        {
            if (desde.Date > hasta.Date)
                throw new ArgumentException(
                    "La fecha desde no puede ser posterior a la fecha hasta.");

            ArgumentNullException.ThrowIfNull(salasIds);

            _logger.LogInformation(
                "Calculando disponibilidad de eventos desde {Desde} hasta {Hasta}. Salas: {Cantidad}",
                desde.Date,
                hasta.Date,
                salasIds.Count);

            // 1. Obtener el calendario activo.
            var calendario = await _repositorioCalendario
                .ObtenerCalendarioActivoAsync()
                ?? throw new InvalidOperationException(
                    "No hay calendario activo del museo.");

            // 2. Obtener las actividades existentes.
            var actividadesEnRango =
                await ObtenerActividadesConConflictoPotencial(desde, hasta);

            // 3. Preparar las actividades para el motor.
            var existingActivities = _availabilityFactory.Create(
                actividadesEnRango.Cast<ActividadMuseoEntity>().ToList(),
                desde,
                hasta);

            // 4. Obtener las salas una sola vez.
            var salas = await _repositorioSalaMuseo
                .ObtenerSalasporIdsAsync(salasIds);

            // 5. Generar candidatos en las franjas realmente operativas.
            var candidatosEventos = new List<CandidateEntry>();

            for (var fecha = desde.Date;
                 fecha <= hasta.Date;
                 fecha = fecha.AddDays(1))
            {
                var franjasOperativas = calendario
                    .ObtenerFranjasOperativas(DateOnly.FromDateTime(fecha))
                    .ToList();

                foreach (var franja in franjasOperativas)
                {
                    for (var inicioSlot = franja.Inicio;
                         inicioSlot.AddMinutes(30) <= franja.Fin;
                         inicioSlot = inicioSlot.AddMinutes(30))
                    {
                        var timeSlot = new TimeSlot(
                            inicioSlot,
                            inicioSlot.AddMinutes(30));

                        var eventoFicticio = new Evento(
                            nombreyApellidoSolicitante: "Disponibilidad",
                            telefonoSolicitante: new Telefono("3511234567"),
                            emailSolicitante: new Email("disponibilidad@museo.com"),
                            institucion: "Museo",
                            tipoEvento: TipoEvento.Conferencia,
                            tituloEvento: "Evento ficticio",
                            descripcionEvento: "",
                            fundamentacionEvento: "",
                            tipoPublico: new List<TipoPublico>
                            {
                                TipoPublico.General
                            },
                            concurrenciaEstimada: 1,
                            horario: timeSlot,
                            salas: salas,
                            requiereDifusion: false,
                            solicitaFlyer: false,
                            recurrenceRule: null);

                        candidatosEventos.Add(new CandidateEntry(
                            Guid.NewGuid(),
                            eventoFicticio,
                            new List<TimeSlot> { timeSlot }.AsReadOnly(),
                            null,
                            "EventoCandidate"));
                    }
                }
            }

            _logger.LogInformation(
                "Candidatos de eventos generados: {Cantidad}",
                candidatosEventos.Count);

            // 6. Validar la disponibilidad con el motor.
            var contexto = new AvailabilityContext
            {
                Start = desde,
                End = hasta,
                ExistingActivities = existingActivities,
                Metadata = new Dictionary<string, object>
                {
                    { "tipoActividad", TipoActividad.EventoActividadExterna },
                    { "salasIds", salasIds }
                }
            };

            var resultados = await _engine.CheckManyAsync(
                contexto,
                candidatosEventos);

            // 7. Convertir los resultados al DTO.
            var horariosDisponibles = new List<HorarioDisponibleDto>();

            foreach (var candidato in candidatosEventos)
            {
                if (!resultados.TryGetValue(candidato.Id, out var resultado))
                    continue;

                var horario = candidato.TimeSlots.FirstOrDefault();

                if (horario == null)
                    continue;

                horariosDisponibles.Add(new HorarioDisponibleDto
                {
                    Inicio = horario.Inicio,
                    Fin = horario.Fin,
                    Disponible = resultado.IsOk,
                    Mensaje = resultado.IsOk
                        ? string.Empty
                        : resultado.Message
                });
            }

            return new EventoDisponibilidadDto
            {
                Periodo = desde.ToString("yyyy-MM"),
                HorariosDisponibles = horariosDisponibles
            };
        }

        private Task<List<ActividadMuseoEntity>>
            ObtenerActividadesConConflictoPotencial(
                DateTime desde,
                DateTime hasta)
        {
            return _repositorioActividadMuseo.FindAllAsync(desde, hasta);
        }
    }
}
