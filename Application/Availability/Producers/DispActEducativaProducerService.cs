
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

using ActividadMuseoEntity = Domain.ActividadMuseo.Entities.ActividadBase;

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
            List<string> salasIds,
            string? actividadEducativaIdExcluir = null)
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
                await ObtenerActividadesConConflictoPotencial(
                    desde,
                    hasta,
                    actividadEducativaIdExcluir);

            // 3. Preparar las actividades existentes para el motor.
            var existingActivities = _availabilityFactory.Create(
                actividadesEnRango.Cast<ActividadMuseoEntity>().ToList(),
                desde,
                hasta);

            _logger.LogInformation(
                "Actividades existentes preparadas: {Cantidad}",
                existingActivities.Count);

            // 4. Obtener las salas una sola vez.
            var salas = await _repositorioSalaMuseo
                .ObtenerSalasporIdsAsync(salasIds);

            // 5. Generar candidatos de 30 minutos usando las franjas operativas.
            var candidatosEventos = new List<CandidateEntry>();

            for (var fecha = desde.Date;
                 fecha <= hasta.Date;
                 fecha = fecha.AddDays(1))
            {
                var franjasOperativas = calendario
                    .ObtenerFranjasOperativas(DateOnly.FromDateTime(fecha))
                    .ToList();

                if (franjasOperativas.Count == 0)
                    continue;

                foreach (var franja in franjasOperativas)
                {
                    for (var inicioSlot = franja.Inicio;
                         inicioSlot.AddMinutes(30) <= franja.Fin;
                         inicioSlot = inicioSlot.AddMinutes(30))
                    {
                        var timeSlot = new TimeSlot(
                            inicioSlot,
                            inicioSlot.AddMinutes(30));

                        var actividadFicticia = new ActividadEducativa(
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

                        candidatosEventos.Add(new CandidateEntry(
                            Guid.NewGuid(),
                            actividadFicticia,
                            new List<TimeSlot> { timeSlot }.AsReadOnly(),
                            null,
                            "ActividadEducativaCandidate"));
                    }
                }
            }

            _logger.LogInformation(
                "Candidatos de actividades educativas generados: {Cantidad}",
                candidatosEventos.Count);

            // 6. Validar candidatos con el motor de disponibilidad.
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

            // 7. Convertir los resultados al DTO de salida.
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

        // Valida una actividad educativa concreta.
        // Se utiliza tanto para crear como para actualizar actividades.
        public async Task<bool> EstaDisponibleAsync(
            ActividadEducativa actividad,
            string? actividadIdExcluir = null)
        {
            ArgumentNullException.ThrowIfNull(actividad);

            var desde = actividad.Horario.Inicio;
            var hasta = actividad.Horario.Fin;

            // 1. Obtener el calendario activo.
            var calendario = await _repositorioCalendario
                .ObtenerCalendarioActivoAsync()
                ?? throw new InvalidOperationException(
                    "No hay calendario activo del museo.");

            // 2. Comprobar que el horario esté dentro de una franja operativa.
            var franjasOperativas = calendario
                .ObtenerFranjasOperativas(DateOnly.FromDateTime(desde))
                .ToList();

            var horarioOperativo = franjasOperativas.Any(franja =>
                desde >= franja.Inicio &&
                hasta <= franja.Fin);

            if (!horarioOperativo)
                return false;

            // 3. Obtener actividades existentes excluyendo la actual.
            var actividadesEnRango =
                await ObtenerActividadesConConflictoPotencial(
                    desde,
                    hasta,
                    actividadIdExcluir);

            // 4. Preparar actividades existentes para el motor.
            var existingActivities = _availabilityFactory.Create(
                actividadesEnRango.Cast<ActividadMuseoEntity>().ToList(),
                desde,
                hasta);

            // 5. Crear un único candidato con la actividad recibida.
            var candidato = new CandidateEntry(
                Guid.NewGuid(),
                actividad,
                new List<TimeSlot> { actividad.Horario }.AsReadOnly(),
                null,
                "ActividadEducativaCandidate");

            var candidatos = new List<CandidateEntry>
            {
                candidato
            };

            // 6. Crear el contexto de disponibilidad.
            var contexto = new AvailabilityContext
            {
                Start = desde,
                End = hasta,
                ExistingActivities = existingActivities,
                Metadata = new Dictionary<string, object>
                {
                    { "tipoActividad", TipoActividad.EventoActividadExterna },
                    {
                        "salasIds",
                        actividad.Salas.Select(sala => sala.Id).ToList()
                    }
                }
            };

            // 7. Validar el candidato.
            var resultados = await _engine.CheckManyAsync(
                contexto,
                candidatos);

            // 8. Devolver el resultado.
            return resultados.TryGetValue(candidato.Id, out var resultado)
                && resultado.IsOk;
        }

        private async Task<List<ActividadMuseoEntity>>
            ObtenerActividadesConConflictoPotencial(
                DateTime desde,
                DateTime hasta,
                string? actividadIdExcluir = null)
        {
            var actividades = await _repositorioActividadMuseo
                .FindAllAsync(desde, hasta);

            // Excluir la actividad que se está actualizando.
            if (!string.IsNullOrWhiteSpace(actividadIdExcluir))
            {
                actividades = actividades
                    .Where(actividad => actividad.Id != actividadIdExcluir)
                    .ToList();
            }

            return actividades;
        }
    }
}
