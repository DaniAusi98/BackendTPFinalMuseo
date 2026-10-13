using Application.Availability.Factories;
using Application.Availability.Models;
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
        ActividadMuseo.Repositories.IRepositorioCalendarioMuseo repositorioCalendario,
        IRepositorioSala repositorioSala,
        IRepositorioTematicas repositorioTematicas,
        ActivityAvailabilityFactory availabilityFactory, // <--- 2) INYECTAMOS EN EL CONSTRUCTOR


        AvailabilityEngine engine,
        ILogger<GuidedAvailabilityProducerService> logger)
    {
        private readonly IServicioDisponibilidadTurnosVisitasGuiadas _servicioGuiadas = servicioGuiadas;
        private readonly IRepositorioGuia _repositorioGuia = repositorioGuia;
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada;
        private readonly ActividadMuseo.Repositories.IRepositorioActividadMuseo _repositorioActividadMuseo = repositorioActividadMuseo;
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorioConfiguracion = repositorioConfiguracion;
        private readonly ActividadMuseo.Repositories.IRepositorioCalendarioMuseo _repositorioCalendario = repositorioCalendario;
        private readonly ActivityAvailabilityFactory _availabilityFactory = availabilityFactory; // <--- 1) AGREGAMOS EL CAMPO
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

            // ============================================================
            // 1) PREFETCH
            // ============================================================

            var guias =
                await _repositorioGuia.ObtenerGuiasConDisponibilidadAsync();

            var visitasGuiadasExistentes =
                await _repositorioVisitaGuiada.GetAllGroupVisitAsync(
                    desde,
                    hasta);
            //en caso de que se pase un id de visita, se filtra para no tener en cuenta la misma visita cuando se consulta disponiblidad en una reprogramacion 
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
                    "No hay configuraci�n activa para visitas guiadas.");
            }

            if (calendario == null)
            {
                throw new InvalidOperationException(
                    "No hay calendario activo del museo.");
            }

            // ============================================================
            // 2) GENERAR TURNOS CANDIDATOS
            // ============================================================

            var candidatosTurnos =
                await _servicioGuiadas.CalcularDisponibilidad(
                    desde,
                    hasta,
                    guias,
                    visitasGuiadasExistentes,
                    configuracion,
                    calendario);

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Servicio devolvi� {Cantidad} turnos",
                candidatosTurnos.Count);

            candidatosTurnos = candidatosTurnos
                .Where(x => x.EstadoTurno == EstadoTurno.Disponible)
                .ToList();

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Turnos disponibles despu�s del filtro: {Cantidad}",
                candidatosTurnos.Count);

            // ============================================================
            // 3) OBTENER ACTIVIDADES EXISTENTES
            //
            // IMPORTANTE:
            // El repositorio debe traer:
            //
            // - Actividad
            // - TimeSlots
            // - Exceptions
            //
            // RecurrenceRule es parte de la actividad, no necesita Include
            // si est� configurada como owned/value object.
            // ============================================================

            var actividadesEnRango =
                await _repositorioActividadMuseo
                    .FindAllAsync(desde, hasta);

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Actividades existentes: {Cantidad}",
                actividadesEnRango.Count);

            // ============================================================
            // 4) PREPARAR ACTIVIDADES EXISTENTES PARA EL ENGINE
            //
            // Ac� ocurre:
            //
            // Sin recurrencia:
            //      Actividad + 1 slot
            //
            // Con recurrencia:
            //      Actividad
            //          ?
            //      RecurrenceExpander
            //          ?
            //      slots generados dentro de la ventana
            //          ?
            //      CandidateEntry
            //
            // ============================================================

            var existingActivities = _availabilityFactory.Create(
               [.. actividadesEnRango],
               desde,
               hasta
           );
            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Existing CandidateEntries preparados: {Cantidad}",
                existingActivities.Count);

            // ============================================================
            // 5) CREAR CANDIDATOS DE VISITAS GUIADAS
            //
            // Cada turno tiene UN SOLO TimeSlot.
            // ============================================================

            List<Sala> salas;

            if (tematicasIds is { Count: > 0 })
            {
                // Obtener las salas asociadas a las temáticas seleccionadas.
                List<TematicaVisita> tematicas = await _repositorioTematicas.GetByIdsAsync(tematicasIds);
                salas = [.. tematicas
                .SelectMany(t => t.Salas)
                .DistinctBy(s => s.Id)];
            }
            else
            {
                // Obtener todas las salas habilitadas para visitas guiadas.
                salas = await _repositorioSala.ObtenerSalasVisitasGrupalesAsync();
            }

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

                    // Este candidato NO es recurrente.
                    // Tiene solamente el turno que estamos verificando.
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

            // ============================================================
            // 6) CONTEXTO BASE
            //
            // Ahora ExistingActivities ya no deber�a ser la fuente
            // principal para solapamientos.
            //
            // El engine necesita trabajar con existingEntries.
            // ============================================================

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

            // ============================================================
            // 7) VALIDAR CON AVAILABILITY ENGINE
            // ============================================================

            var results =
                await _engine.CheckManyAsync(
                    baseCtx,
                    entries);

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] AvailabilityEngine devolvi� {Cantidad} resultados",
                results.Count);

            // ============================================================
            // 8) RECUPERAR TURNOS APROBADOS
            // ============================================================

            var aprobados = new List<TurnoDisponible>();

            foreach (var entry in entries)
            {
                if (!results.TryGetValue(
                    entry.Id,
                    out var result))
                {
                    _logger.LogWarning(
                        "[GuidedAvailabilityProducer] No se encontr� resultado para {Id}",
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
    }
}