using Application.ActividadMuseo.Repositories;
using Application.Availability;
using Application.Availability.Factories; // Inyectamos la ubicación del Factory
using Application.Availability.Models;
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
    /// <summary>
    /// Servicio que produce disponibilidad de horas para eventos.
    /// Genera candidatos hora por hora dentro del horario de apertura del museo
    /// para un rango de fechas y valida su disponibilidad usando el AvailabilityEngine.
    /// </summary>
    public class EventAvailabilityProducerService
    {
        private readonly IRepositorioActividadMuseo _repositorioActividadMuseo;
        private readonly IRepositorioCalendarioMuseo _repositorioCalendario;
        private readonly IRepositorioSala _repositorioSalaMuseo;
        private readonly AvailabilityEngine _engine;
        private readonly ActivityAvailabilityFactory _availabilityFactory; // <--- 1) AGREGAMOS EL CAMPO
        private readonly ILogger<EventAvailabilityProducerService> _logger;

        public EventAvailabilityProducerService(
            IRepositorioActividadMuseo repositorioActividadMuseo,
            IRepositorioCalendarioMuseo repositorioCalendario,
            IRepositorioSala repositorioSalaMuseo,
            AvailabilityEngine engine,
            ActivityAvailabilityFactory availabilityFactory, // <--- 2) INYECTAMOS EN EL CONSTRUCTOR
            ILogger<EventAvailabilityProducerService> logger)
        {
            _repositorioActividadMuseo = repositorioActividadMuseo;
            _repositorioCalendario = repositorioCalendario;
            _repositorioSalaMuseo = repositorioSalaMuseo;
            _engine = engine;
            _availabilityFactory = availabilityFactory; // <--- 3) ASIGNAMOS LA INSTANCIA DI
            _logger = logger;
        }

        /// <summary>
        /// Calcula la disponibilidad hora por hora para eventos en un rango de fechas.
        /// </summary>
        public async Task<EventoDisponibilidadDto> GetEventAvailabilityAsync(
            DateTime desde,
            DateTime hasta,
            List<string> salasIds)
        {
            _logger.LogInformation(
                "[EventAvailabilityProducer] Calculando disponibilidad para período: {Desde} - {Hasta}, " +
                "salas: {SalasCount}",
                desde.Date,
                hasta.Date,
                salasIds.Count);

            // ============================================================
            // 1) OBTENER CONFIGURACIÓN DEL MUSEO
            // ============================================================
            var calendario = await _repositorioCalendario.ObtenerCalendarioActivoAsync();
            if (calendario == null)
            {
                throw new InvalidOperationException("No hay calendario activo del museo.");
            }

            _logger.LogInformation(
                "[EventAvailabilityProducer] Horario museo: {Inicio} - {Fin}",
                calendario.HorarioApertura.HoraInicio,
                calendario.HorarioApertura.HoraFin);

            // ============================================================
            // 2) OBTENER ACTIVIDADES EXISTENTES EN EL RANGO
            // ============================================================
            var actividadesEnRango = await ObtenerActividadesConConflictoPotencial(desde, hasta);

            _logger.LogInformation(
                "[EventAvailabilityProducer] Actividades existentes en rango: {Cantidad}",
                actividadesEnRango.Count);

            // ============================================================
            // 4) PREPARAR ACTIVIDADES EXISTENTES PARA EL ENGINE (CORREGIDO)
            // ============================================================
            // Usamos la instancia inyectada '_availabilityFactory' en vez de la llamada estática vieja
            var existingActivities = _availabilityFactory.Create(
                actividadesEnRango.Cast<ActividadMuseoEntity>().ToList(),
                desde,
                hasta
            );

            _logger.LogInformation(
                "[EventAvailabilityProducer] Actividades existentes preparadas: {Cantidad}",
                existingActivities.Count);

            // ============================================================
            // 5) GENERAR CANDIDATOS HORA POR HORA
            // ============================================================
            var candidatosEventos = new List<CandidateEntry>();

            for (var fecha = desde.Date; fecha.Date <= hasta.Date; fecha = fecha.AddDays(1))
            {
                if (!calendario.EsDiaOperativo(fecha))
                {
                    _logger.LogDebug("[EventAvailabilityProducer] Día no operativo: {Fecha}", fecha.Date);
                    continue;
                }

                var horaInicio = calendario.HorarioApertura.HoraInicio;
                var horaFin = calendario.HorarioApertura.HoraFin;

                for (var hora = horaInicio; hora < horaFin; hora = hora.AddMinutes(30))
                {
                    var inicioSlot = new DateTime(fecha.Year, fecha.Month, fecha.Day, hora.Hour, hora.Minute, hora.Second);
                    var finSlot = inicioSlot.AddMinutes(30);

                    if (finSlot > hasta.AddDays(1))
                        break;

                    var timeSlot = new TimeSlot(inicioSlot, finSlot);
                    var eventoFicticio = CrearEventoFicticio(timeSlot, salasIds);

                    var candidato = new CandidateEntry(
                        Guid.NewGuid(),
                        await eventoFicticio,
                        new[] { timeSlot }.ToList().AsReadOnly(),
                        null,
                        "EventoCandidate");

                    candidatosEventos.Add(candidato);
                }
            }

            _logger.LogInformation("[EventAvailabilityProducer] Candidatos generados: {Cantidad}", candidatosEventos.Count);

            // ============================================================
            // 6) VALIDAR DISPONIBILIDAD CON EL ENGINE
            // ============================================================
            var contexto = new AvailabilityContext
            {
                Start = desde,
                End = hasta,
                ExistingActivities = existingActivities,
                Metadata = new Dictionary<string, object>
                {
                    { "tipoActividad", TipoActividad.Evento },
                    { "salasIds", salasIds }
                }
            };

            var resultados = await _engine.CheckManyAsync(contexto, candidatosEventos);

            _logger.LogInformation("[EventAvailabilityProducer] Validación completada. Resultados: {Cantidad}", resultados.Count);

            // ============================================================
            // 7) PROCESAR RESULTADOS DEL ENGINE
            // ============================================================
            var horariosDisponibles = new List<HorarioDisponibleDto>();

            foreach (var kvp in resultados)
            {
                var candidatoId = kvp.Key;
                var resultado = kvp.Value;

                var candidato = candidatosEventos.FirstOrDefault(c => c.Id == candidatoId);
                if (candidato == null) continue;

                var horario = candidato.TimeSlots.FirstOrDefault();
                if (horario == null) continue;

                var disponibilidad = new HorarioDisponibleDto
                {
                    // Ajustado a las propiedades de tu objeto TimeSlot corregido (Inicio/Fin)
                    Inicio = horario.Inicio,
                    Fin = horario.Fin,
                    Disponible = resultado.IsOk,
                    Mensaje = resultado.IsOk ? string.Empty : resultado.Message
                };

                horariosDisponibles.Add(disponibilidad);
            }

            var resultadoFinal = new EventoDisponibilidadDto
            {
                Periodo = desde.ToString("yyyy-MM"),
                HorariosDisponibles = horariosDisponibles
            };

            return resultadoFinal;
        }

        private async Task<List<ActividadMuseoEntity>> ObtenerActividadesConConflictoPotencial(DateTime desde, DateTime hasta)
        {
            var actividadesDirectas = await _repositorioActividadMuseo.FindAllAsync(desde, hasta); return actividadesDirectas;
        }
        private async Task<Evento> CrearEventoFicticio(TimeSlot timeSlot, List<string> salasIds)
        {
            var salas = await _repositorioSalaMuseo.ObtenerSalasporIdsAsync(salasIds);
            var evento = new Evento(nombreyApellidoSolicitante: "Disponibilidad", telefonoSolicitante: new Telefono("3511234567"), emailSolicitante: new Email("disponibilidad@museo.com"), institucion: "Museo", tipoEvento: TipoEvento.Conferencia, tituloEvento: "Evento ficticio", descripcionEvento: "", fundamentacionEvento: "", tipoPublico: new List<TipoPublico> { TipoPublico.General }, concurrenciaEstimada: 1, horario: timeSlot, salas: salas, requiereDifusion: false, solicitaFlyer: false, recurrenceRule: null); // Se pasa nulo porque el candidato hora por hora evalúa bloques únicos);
                return evento;
        }
    }
}
