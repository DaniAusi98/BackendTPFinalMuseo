using Application.Availability.Factories;
using Application.Availability.Models;
using Application.ConfiguracionSistema.Repositories;
using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.Repositories;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;
using Domain.VisitasGrupales.DomainServices;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Microsoft.Extensions.Logging;
using static Domain.VisitasGrupales.Entities.GrupalGuiada.Enums.Enums;

namespace Application.Availability.Producers
{
    public class GuidedAvailabilityProducerService(
        IServicioDisponibilidadTurnosVisitasGuiadas servicioGuiadas,
        IRepositorioGuia repositorioGuia,
        IRepositorioVisitaGuiada repositorioVisitaGuiada,
        ActividadMuseo.Repositories.IRepositorioActividadMuseo repositorioActividadMuseo,
        IRepositorioConfiguracionVisitasGrupalesGuiadas repositorioConfiguracion,
        IRepositorioCalendarioMuseo repositorioCalendario,
        IRepositorioSala repositorioSala,
        IRepositorioTematicas repositorioTematicas,
        ActivityAvailabilityFactory availabilityFactory,
        AvailabilityEngine engine,
        ILogger<GuidedAvailabilityProducerService> logger)
    {
        private readonly IServicioDisponibilidadTurnosVisitasGuiadas _servicioGuiadas = servicioGuiadas;
        private readonly IRepositorioGuia _repositorioGuia = repositorioGuia;
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada;
        private readonly ActividadMuseo.Repositories.IRepositorioActividadMuseo _repositorioActividadMuseo = repositorioActividadMuseo;
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorioConfiguracion = repositorioConfiguracion;
        private readonly IRepositorioCalendarioMuseo _repositorioCalendario = repositorioCalendario;
        private readonly ActivityAvailabilityFactory _availabilityFactory = availabilityFactory;
        private readonly AvailabilityEngine _engine = engine;
        private readonly IRepositorioTematicas _repositorioTematicas = repositorioTematicas;
        private readonly IRepositorioSala _repositorioSala = repositorioSala;
        private readonly ILogger<GuidedAvailabilityProducerService> _logger = logger;

        public async Task<List<TurnoDisponible>> GetAvailableTurnsAsync(
            DateTime desde,
            DateTime hasta,
            string? visitaId = null,
            List<string>? tematicasIds = null)
        {
            _logger.LogInformation(
                "[GuidedAvailabilityProducer] ===== INICIO =====");

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Rango solicitado: Desde {Desde} - Hasta {Hasta}",
                desde,
                hasta);

            // 1) PREFETCH

            var guias =
                await _repositorioGuia.ObtenerGuiasConDisponibilidadAsync();

            var visitasGuiadasExistentes =
                await _repositorioVisitaGuiada.GetActiveGuidedTours(
                    desde,
                    hasta);

            // Excluir la visita original en caso de reprogramación.
            if (!string.IsNullOrEmpty(visitaId))
            {
                visitasGuiadasExistentes = visitasGuiadasExistentes
                    .Where(v => v.Id != visitaId)
                    .ToList();
            }

            var configuracion =
                await _repositorioConfiguracion
                    .ObtenerConfiguracionActivaAsync();

            var calendario =
                await _repositorioCalendario
                    .ObtenerCalendarioActivoAsync();

            if (configuracion == null)
            {
                throw new InvalidOperationException(
                    "No hay configuración activa para visitas guiadas.");
            }

            if (calendario == null)
            {
                throw new InvalidOperationException(
                    "No hay calendario activo del museo.");
            }

            // 2) GENERAR TURNOS CANDIDATOS

            var candidatosTurnos =
                await _servicioGuiadas.CalcularDisponibilidad(
                    desde,
                    hasta,
                    guias,
                    visitasGuiadasExistentes,
                    configuracion,
                    calendario);

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Servicio devolvió {Cantidad} turnos",
                candidatosTurnos.Count);

            candidatosTurnos = candidatosTurnos
                .Where(x => x.EstadoTurno == EstadoTurno.Disponible)
                .ToList();

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Turnos disponibles después del filtro: {Cantidad}",
                candidatosTurnos.Count);

            var actividadesEnRango =
                await _repositorioActividadMuseo
                    .FindAllAsync(desde, hasta);

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Actividades existentes: {Cantidad}",
                actividadesEnRango.Count);

            // 3) PREPARAR ACTIVIDADES EXISTENTES PARA EL ENGINE

            var existingActivities = _availabilityFactory.Create(
                [.. actividadesEnRango],
                desde,
                hasta
            );

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Existing CandidateEntries preparados: {Cantidad}",
                existingActivities.Count);

            // 4) OBTENER SALAS

            List<Sala> salas;

            if (tematicasIds is { Count: > 0 })
            {
                List<TematicaVisita> tematicas =
                    await _repositorioTematicas.GetByIdsAsync(tematicasIds);

                salas = [.. tematicas
                    .SelectMany(t => t.Salas)
                    .DistinctBy(s => s.Id)];
            }
            else
            {
                salas =
                    await _repositorioSala.ObtenerSalasVisitasGrupalesAsync();
            }

            // 5) CREAR CANDIDATOS DE VISITAS GUIADAS

            var entries = new List<CandidateEntry>();

            var email = new Email("sistema@localhost.com");
            var telefono = new Telefono("1123456789");

            foreach (var turno in candidatosTurnos)
            {
                var timeSlot = new TimeSlot(
                    turno.HorarioTurno.Inicio,
                    turno.HorarioTurno.Fin);

                var candidate = new VisitaGrupalGuiada(
                    usuarioVisitanteId: "system",
                    nombreSolicitante: "Sistema",
                    apellidoSolicitante: "Sistema",
                    emailSolicitante: email,
                    telefonoSolicitante: telefono,
                    nivelEducativo: null,
                    anioGrado: null,
                    cantidadPersonas: 1,
                    institucion: "Sistema",
                    emailInstitucion: email,
                    telefonoInstitucion: telefono,
                    paisInstitucion: "",
                    provinciaInstitucion: "",
                    ciudadInstitucion: "",
                    descripcionDiversidad: string.Empty,
                    motivoVisita: string.Empty,
                    observaciones: string.Empty,
                    horario: timeSlot,
                    tematicas: Array.Empty<TematicaVisita>(),
                    salas: salas
                );

                var entry = new CandidateEntry(
                    Id: Guid.NewGuid(),
                    Candidate: candidate,
                    TimeSlots: new List<TimeSlot>
                    {
                        timeSlot
                    },
                    Original: turno,
                    Source: "GuidedService"
                );

                entries.Add(entry);
            }

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] CandidateEntry creados: {Cantidad}",
                entries.Count);

            if (entries.Count == 0)
            {
                _logger.LogWarning(
                    "[GuidedAvailabilityProducer] No hay candidatos.");

                return new List<TurnoDisponible>();
            }

            // 6) CONTEXTO BASE

            var baseCtx = new AvailabilityContext
            {
                Start = desde,
                End = hasta,

                ExistingActivities = existingActivities,

                Metadata = new Dictionary<string, object>
                {
                    {
                        AvailabilityMetadataKeys.GuidesPrefetched,
                        guias
                    }
                }
            };

            // 7) VALIDAR CON AVAILABILITY ENGINE

            var results =
                await _engine.CheckManyAsync(
                    baseCtx,
                    entries);

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] AvailabilityEngine devolvió {Cantidad} resultados",
                results.Count);

            // 8) RECUPERAR TURNOS APROBADOS

            var aprobados = new List<TurnoDisponible>();

            foreach (var entry in entries)
            {
                if (!results.TryGetValue(entry.Id, out var result))
                {
                    _logger.LogWarning(
                        "[GuidedAvailabilityProducer] No se encontró resultado para {Id}",
                        entry.Id);

                    continue;
                }

                if (!result.IsOk)
                {
                    _logger.LogDebug(
                        "[GuidedAvailabilityProducer] Candidato {Id} rechazado: {Message}",
                        entry.Id,
                        result.Message);

                    continue;
                }

                if (entry.Original is TurnoDisponible turno)
                {
                    aprobados.Add(turno);
                }
            }

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] ===== FIN =====");

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Total aprobados: {Cantidad}",
                aprobados.Count);

            return aprobados;
        }

        // ============================================================
        // VALIDAR UN ÚNICO TURNO
        // ============================================================

        public async Task<bool> IsTurnAvailableAsync(
            TimeSlot horario,
            string? visitaId = null,
            List<string>? tematicasIds = null)
        {
            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Validando turno {Inicio} - {Fin}",
                horario.Inicio,
                horario.Fin);

            // 1) PREFETCH

            var guias =
                await _repositorioGuia.ObtenerGuiasConDisponibilidadAsync();

            var visitasGuiadasExistentes =
                await _repositorioVisitaGuiada.GetActiveGuidedTours(
                    horario.Inicio,
                    horario.Fin);

            // Excluir la visita original si es una reprogramación.
            if (!string.IsNullOrEmpty(visitaId))
            {
                visitasGuiadasExistentes = visitasGuiadasExistentes
                    .Where(v => v.Id != visitaId)
                    .ToList();
            }

            var configuracion =
                await _repositorioConfiguracion
                    .ObtenerConfiguracionActivaAsync();

            var calendario =
                await _repositorioCalendario
                    .ObtenerCalendarioActivoAsync();

            if (configuracion == null)
            {
                throw new InvalidOperationException(
                    "No hay configuración activa para visitas guiadas.");
            }

            if (calendario == null)
            {
                throw new InvalidOperationException(
                    "No hay calendario activo del museo.");
            }

            // 2) VALIDAR DISPONIBILIDAD PROPIA DE VISITAS GUIADAS

            var disponible =
                await _servicioGuiadas.ValidarDisponibilidadTurno(
                    horario,
                    guias,
                    visitasGuiadasExistentes,
                    configuracion,
                    calendario);

            if (!disponible)
            {
                _logger.LogInformation(
                    "[GuidedAvailabilityProducer] Turno rechazado por el servicio de visitas guiadas.");

                return false;
            }

            // 3) OBTENER ACTIVIDADES EXISTENTES

            var actividadesEnRango =
                await _repositorioActividadMuseo.FindAllAsync(
                    horario.Inicio,
                    horario.Fin);

            // Excluir también la visita original de las actividades generales.
            if (!string.IsNullOrEmpty(visitaId))
            {
                actividadesEnRango = actividadesEnRango
                    .Where(a => a.Id != visitaId)
                    .ToList();
            }

            var existingActivities = _availabilityFactory.Create(
                [.. actividadesEnRango],
                horario.Inicio,
                horario.Fin
            );

            // 4) OBTENER SALAS

            List<Sala> salas;

            if (tematicasIds is { Count: > 0 })
            {
                List<TematicaVisita> tematicas =
                    await _repositorioTematicas.GetByIdsAsync(tematicasIds);

                salas = [.. tematicas
                    .SelectMany(t => t.Salas)
                    .DistinctBy(s => s.Id)];
            }
            else
            {
                salas =
                    await _repositorioSala.ObtenerSalasVisitasGrupalesAsync();
            }

            // 5) PREPARAR EL CANDIDATO

            var email = new Email("sistema@localhost.com");
            var telefono = new Telefono("1123456789");

            var candidate = new VisitaGrupalGuiada(
                usuarioVisitanteId: "system",
                nombreSolicitante: "Sistema",
                apellidoSolicitante: "Sistema",
                emailSolicitante: email,
                telefonoSolicitante: telefono,
                nivelEducativo: null,
                anioGrado: null,
                cantidadPersonas: 1,
                institucion: "Sistema",
                emailInstitucion: email,
                telefonoInstitucion: telefono,
                paisInstitucion: "",
                provinciaInstitucion: "",
                ciudadInstitucion: "",
                descripcionDiversidad: string.Empty,
                motivoVisita: string.Empty,
                observaciones: string.Empty,
                horario: horario,
                tematicas: Array.Empty<TematicaVisita>(),
                salas: salas
            );

            var entry = new CandidateEntry(
                Id: Guid.NewGuid(),
                Candidate: candidate,
                TimeSlots: new List<TimeSlot>
                {
                    horario
                },
                Original: horario,
                Source: "GuidedService"
            );

            // 6) CONTEXTO BASE

            var baseCtx = new AvailabilityContext
            {
                Start = horario.Inicio,
                End = horario.Fin,

                ExistingActivities = existingActivities,

                Metadata = new Dictionary<string, object>
                {
                    {
                        AvailabilityMetadataKeys.GuidesPrefetched,
                        guias
                    }
                }
            };

            // 7) VALIDAR CON AVAILABILITY ENGINE

            var results = await _engine.CheckManyAsync(
                baseCtx,
                new List<CandidateEntry> { entry });

            if (!results.TryGetValue(entry.Id, out var result))
            {
                _logger.LogWarning(
                    "[GuidedAvailabilityProducer] No se encontró resultado para el candidato {Id}",
                    entry.Id);

                return false;
            }

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Resultado del turno: {Disponible}. {Mensaje}",
                result.IsOk,
                result.Message);

            return result.IsOk;
        }
    }
}