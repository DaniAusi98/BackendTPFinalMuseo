
using Application.Availability.Factories;
using Application.Availability.Models;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.Repositories;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.DomainServices;
using Domain.VisitasGrupales.Entities;

using static Domain.VisitasGrupales.Enums.Enums;

namespace Application.Availability.Producers
{
    /// <summary>
    /// Producer que genera bloques horarios para visitas grupales autoguiadas
    /// y valida las candidatas contra el AvailabilityEngine.
    /// </summary>
    public class SelfGuidedAvailabilityProducerService(
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,
        IRepositorioConfiguracionHorarioAutoguiada repositorioConfiguracionHorarioAutoguiada,
        ActividadMuseo.Repositories.IRepositorioCalendarioMuseo repositorioCalendario,
        ActividadMuseo.Repositories.IRepositorioActividadMuseo repositorioActividadMuseo,
        IServicioDisponibilidadSlotsAutoguiadas servicioDisponibilidadSlotsAutoguiadas,
        IRepositorioSala repositorioSala,
        ActivityAvailabilityFactory availabilityFactory,
        IRepositorioTematicas tematicaRepository,
        AvailabilityEngine engine)
    {
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada = repositorioVisitaGrupalAutoguiada;
        private readonly ActividadMuseo.Repositories.IRepositorioActividadMuseo _repositorioActividadMuseo = repositorioActividadMuseo;
        private readonly IRepositorioConfiguracionHorarioAutoguiada _repositorioConfiguracionHorarioAutoguiada = repositorioConfiguracionHorarioAutoguiada;
        private readonly ActividadMuseo.Repositories.IRepositorioCalendarioMuseo _repositorioCalendario = repositorioCalendario;
        private readonly IRepositorioSala _repositorioSala = repositorioSala;
        private readonly IServicioDisponibilidadSlotsAutoguiadas _servicioDisponibilidadSlotsAutoguiadas = servicioDisponibilidadSlotsAutoguiadas;
        private readonly AvailabilityEngine _engine = engine;
        private readonly ActivityAvailabilityFactory _availabilityFactory = availabilityFactory;        private readonly IRepositorioTematicas _tematicaRepository = tematicaRepository;
        
        public async Task<List<SlotDisponibleVisitaAutoguiada>> GetHourlyBlocksAsync(
            DateTime desde,
            DateTime hasta)
        {
            // ============================================================
            // 1) OBTENER DATOS NECESARIOS PARA GENERAR LOS SLOTS
            // ============================================================

            var visitasAutoguiadasExistentes =
                await _repositorioVisitaGrupalAutoguiada
                    .GetAllGroupVisitAuAsync(desde, hasta);

            var configuracion =
                await _repositorioConfiguracionHorarioAutoguiada
                    .ObtenerConfiguracionActivaAsync();

            var calendario =
                await _repositorioCalendario
                    .ObtenerCalendarioActivoAsync();

            if (configuracion == null)
            {
                throw new InvalidOperationException(
                    "No hay configuraci�n activa para visitas autoguiadas.");
            }

            if (calendario == null)
            {
                throw new InvalidOperationException(
                    "No hay calendario activo del museo.");
            }

            // ============================================================
            // 2) GENERAR SLOTS CANDIDATOS
            // ============================================================

            var candidatosTurnos =
                await _servicioDisponibilidadSlotsAutoguiadas
                    .CalcularDisponibilidad(
                        desde,
                        hasta,
                        visitasAutoguiadasExistentes,
                        configuracion,
                        calendario);

            candidatosTurnos = candidatosTurnos
                .Where(turno =>
                    turno.EstadoSlot == EstadoTurno.Disponible)
                .ToList();

            // ============================================================
            // 3) OBTENER ACTIVIDADES EXISTENTES
            // ============================================================

            var actividadesExistentes =
                await _repositorioActividadMuseo
                    .FindAllAsync(desde, hasta);

            // ============================================================
            // 4) EXPANDIR ACTIVIDADES EXISTENTES
            //
            // Sin recurrencia:
            //      Actividad -> 1 TimeSlot
            //
            // Con recurrencia:
            //      Actividad
            //          ?
            //      RecurrenceExpander
            //          ?
            //      m�ltiples TimeSlots
            //
            // ============================================================

           

            var existingActivities =
                _availabilityFactory.Create(
                    actividadesExistentes,
                    desde,
                    hasta);

            // ============================================================
            // 5) CREAR CANDIDATOS
            // ============================================================

            var entries =
                new List<CandidateEntry>();

            var email =
                new Email("sistema@localhost.com");

            var telefono =
                new Telefono("1123456789");

            foreach (var turno in candidatosTurnos)
            {
                var timeSlot =
                    new TimeSlot(
                        turno.HorarioSlot.Inicio,
                        turno.HorarioSlot.Fin);

                var candidate =
                    new VisitaGrupalAutoguiada(
                        usuarioVisitanteId: "system",
                        cantidadPersonas: turno.cuposDisponibles,
                        institucion: "Sistema",
                        emailInstitucion: email,
                        paisInstitucion: "Sistema",
                        provinciaInstitucion: "Sistema",
                        ciudadInstitucion: "Sistema",
                        descripcionDiversidad: string.Empty,
                        observaciones: string.Empty,
                        horario: timeSlot,
                        tematicas: [],
                        salas: []
                    );

                var entry =
                    new CandidateEntry(
                        Id: Guid.NewGuid(),

                        Candidate: candidate,

                        // Una autoguiada candidata representa
                        // solamente este slot.
                        TimeSlots: new List<TimeSlot>
                        {
                            timeSlot
                        },

                        Original: turno,

                        Source: "SelfGuidedService"
                    );

                entries.Add(entry);
            }

            if (entries.Count == 0)
            {
                return new List<SlotDisponibleVisitaAutoguiada>();
            }

            // ============================================================
            // 6) CONTEXTO BASE
            // ============================================================

            var baseCtx =
                new AvailabilityContext
                {
                    Start = desde,
                    End = hasta,

                    ExistingActivities =
                        existingActivities,

                    Metadata =
                        new Dictionary<string, object>()
                };

            // ============================================================
            // 7) VALIDAR CANDIDATOS
            // ============================================================

            var results =
                await _engine.CheckManyAsync(
                    baseCtx,
                    entries);

            // ============================================================
            // 8) RECUPERAR SLOTS APROBADOS
            // ============================================================

            var approved =
                new List<SlotDisponibleVisitaAutoguiada>();

            foreach (var entry in entries)
            {
                if (!results.TryGetValue(
                    entry.Id,
                    out var result))
                {
                    continue;
                }

                if (!result.IsOk)
                {
                    continue;
                }

                if (entry.Original
                    is SlotDisponibleVisitaAutoguiada slot)
                {
                    approved.Add(slot);
                }
            }

            return approved;
        }
    }
}

