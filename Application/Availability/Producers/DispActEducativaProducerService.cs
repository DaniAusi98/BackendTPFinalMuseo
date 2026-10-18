
using Application.ActividadesEducativas.DataTransferObjets;
using Application.ActividadMuseo.Repositories;
using Application.Availability.Factories;
using Application.Availability.Models;
using Application.ConfiguracionSistema.Repositories;
using Application.MuseumResources.Repositories;
using Domain.ActividadesAreaEducacion.Entities;
using Domain.Common.ValueObjets;
using Microsoft.Extensions.Logging;
using static Domain.ActividadesAreaEducacion.Enums.Enums;
using static Domain.ActividadMuseo.Enums.Enums;
using ActividadMuseoEntity = Domain.ActividadMuseo.Entities.ActividadMuseo;

namespace Application.Availability.Producers
{
    public class DispActEducativaProducerService
    {
        private readonly IRepositorioActividadMuseo _repositorioActividadMuseo;
        private readonly IRepositorioCalendarioMuseo _repositorioCalendario;
        private readonly IRepositorioSala _repositorioSalaMuseo;
        private readonly AvailabilityEngine _engine;
        private readonly ActivityAvailabilityFactory _availabilityFactory;
        private readonly ILogger<DispActEducativaProducerService> _logger;

        public DispActEducativaProducerService(
            IRepositorioActividadMuseo repositorioActividadMuseo,
            IRepositorioCalendarioMuseo repositorioCalendario,
            IRepositorioSala repositorioSalaMuseo,
            AvailabilityEngine engine,
            ActivityAvailabilityFactory availabilityFactory,
            ILogger<DispActEducativaProducerService> logger)
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

        public async Task<DispActividadEducativaDto> GetActEducativaAvailabilityAsync(
            DateTime desde,
            DateTime hasta,
            List<string> salasIds)
        {
            if (desde.Date > hasta.Date)
                throw new ArgumentException(
                    "La fecha desde no puede ser posterior a la fecha hasta.");

            ArgumentNullException.ThrowIfNull(salasIds);

            _logger.LogInformation(
                "Calculando disponibilidad de actividades educativas. " +
                "Desde: {Desde}, Hasta: {Hasta}, Salas: {Cantidad}",
                desde.Date,
                hasta.Date,
                salasIds.Count);

            // 1. Obtener el calendario activo.
            var calendario = await _repositorioCalendario
                .ObtenerCalendarioActivoAsync()
                ?? throw new InvalidOperationException(
                    "No hay calendario activo del museo.");

            // 2. Obtener las actividades existentes que pueden generar conflictos.
            var actividadesEnRango =
                await ObtenerActividadesConConflictoPotencial(desde, hasta);

            // 3. Preparar las actividades existentes para el motor.
            var existingActivities = _availabilityFactory.Create(
                actividadesEnRango.Cast<ActividadMuseoEntity>().ToList(),
                desde,
                hasta);

            _logger.LogInformation(
                "Actividades existentes preparadas: {Cantidad}",
                existingActivities.Count);

            // 4. Generar candidatos de 30 minutos usando las franjas
            // operativas reales del calendario.
            var candidatosEventos = new List<CandidateEntry>();

            for (var fecha = desde.Date;
                 fecha <= hasta.Date;
                 fecha = fecha.AddDays(1))
            {
                var franjasOperativas = calendario
                    .ObtenerFranjasOperativas(
                        DateOnly.FromDateTime(fecha))
                    .ToList();

                if (franjasOperativas.Count == 0)
                {
                    _logger.LogDebug(
                        "No hay franjas operativas para {Fecha}",
                        fecha.Date);

                    continue;
                }

                foreach (var franja in franjasOperativas)
                {
                    for (var inicioSlot = franja.Inicio;
                         inicioSlot.AddMinutes(30) <= franja.Fin;
                         inicioSlot = inicioSlot.AddMinutes(30))
                    {
                        var timeSlot = new TimeSlot(
                            inicioSlot,
                            inicioSlot.AddMinutes(30));

                        var eventoFicticio = await CrearEventoFicticio(
                            timeSlot,
                            salasIds);

                        var candidato = new CandidateEntry(
                            Guid.NewGuid(),
                            eventoFicticio,
                            new List<TimeSlot> { timeSlot }.AsReadOnly(),
                            null,
                            "EventoCandidate");

                        candidatosEventos.Add(candidato);
                    }
                }
            }

            _logger.LogInformation(
                "Candidatos generados: {Cantidad}",
                candidatosEventos.Count);

            // 5. Validar candidatos con el motor de disponibilidad.
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

            _logger.LogInformation(
                "Validación completada. Resultados: {Cantidad}",
                resultados.Count);

            // 6. Convertir los resultados al DTO de salida.
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

            return new DispActividadEducativaDto
            {
                Periodo = desde.ToString("yyyy-MM"),
                HorariosDisponibles = horariosDisponibles
            };
        }

        private async Task<List<ActividadMuseoEntity>>
            ObtenerActividadesConConflictoPotencial(
                DateTime desde,
                DateTime hasta)
        {
            return await _repositorioActividadMuseo.FindAllAsync(
                desde,
                hasta);
        }

        private async Task<ActividadEducativa> CrearEventoFicticio(
            TimeSlot timeSlot,
            List<string> salasIds)
        {
            var salas = await _repositorioSalaMuseo
                .ObtenerSalasporIdsAsync(salasIds);

            return new ActividadEducativa(
                titulo: "TituloFicticio",
                tipoActividadEducativa: TipoActividadEducativa.Otros,
                proyectoVinculadaId: "proyectoficticio",
                institucionVinculada: "InstitucionFicticia",
                publicoObjetivo: "publicoFicticio",
                observaciones: "observaciones",
                horario: timeSlot,
                salas: salas,
                requiereDifusion: false,
                solicitaFlyer: false,
                cantidadAsistentes: 15,
                urlImagenes: null,
                rrule: null);
        }
    }
}
