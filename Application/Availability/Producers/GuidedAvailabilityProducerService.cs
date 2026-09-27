using Application.Availability.Models;
using Application.Availability.Factories;
using Application.VisitaGrupal.Repositories;
using Domain.VisitasGrupales.DomainServices;
using Domain.VisitasGrupales.Entities;
using Domain.Common.ValueObjets;
using static Domain.VisitasGrupales.Enums.Enums;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Microsoft.Extensions.Logging;

namespace Application.Availability.Producers
{
    public class GuidedAvailabilityProducerService
    {
        private readonly IServicioDisponibilidadTurnosVisitasGuiadas _servicioGuiadas;
        private readonly IRepositorioGuia _repositorioGuia;
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada;
        private readonly ActividadMuseo.Repositories.IRepositorioActividadMuseo _repositorioActividadMuseo;
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorioConfiguracion;
        private readonly ActividadMuseo.Repositories.IRepositorioCalendarioMuseo _repositorioCalendario;
        private readonly ActivityAvailabilityFactory _availabilityFactory; // <--- 1) AGREGAMOS EL CAMPO
        private readonly AvailabilityEngine _engine;
        private readonly ILogger<GuidedAvailabilityProducerService> _logger;

        public GuidedAvailabilityProducerService(
            IServicioDisponibilidadTurnosVisitasGuiadas servicioGuiadas,
            IRepositorioGuia repositorioGuia,
            IRepositorioVisitaGuiada repositorioVisitaGuiada,
            ActividadMuseo.Repositories.IRepositorioActividadMuseo repositorioActividadMuseo,
            IRepositorioConfiguracionVisitasGrupalesGuiadas repositorioConfiguracion,
            ActividadMuseo.Repositories.IRepositorioCalendarioMuseo repositorioCalendario,
            ActivityAvailabilityFactory availabilityFactory, // <--- 2) INYECTAMOS EN EL CONSTRUCTOR

            AvailabilityEngine engine,
            ILogger<GuidedAvailabilityProducerService> logger)
        {
            _servicioGuiadas = servicioGuiadas;
            _repositorioGuia = repositorioGuia;
            _repositorioVisitaGuiada = repositorioVisitaGuiada;
            _repositorioActividadMuseo = repositorioActividadMuseo;
            _repositorioConfiguracion = repositorioConfiguracion;
            _repositorioCalendario = repositorioCalendario;
            _availabilityFactory = availabilityFactory; // <--- 3) ASIGNAMOS EL CAMPO
            _engine = engine;
            _logger = logger;
        }

        public async Task<List<TurnoDisponible>> GetAvailableTurnsAsync(
            DateTime desde,
            DateTime hasta)
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
                    salas: null
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