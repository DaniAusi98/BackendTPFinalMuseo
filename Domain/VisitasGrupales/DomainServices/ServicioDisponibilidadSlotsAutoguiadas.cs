
using Domain.Common.Entities;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities.GrupalAutoguiada;
using Microsoft.Extensions.Logging;
using static Domain.VisitasGrupales.Entities.GrupalGuiada.Enums.Enums;

namespace Domain.VisitasGrupales.DomainServices
{
    public class ServicioDisponibilidadSlotsAutoguiadas
        : IServicioDisponibilidadSlotsAutoguiadas
    {
        private readonly ILogger<ServicioDisponibilidadSlotsAutoguiadas> _logger;

        public ServicioDisponibilidadSlotsAutoguiadas(
            ILogger<ServicioDisponibilidadSlotsAutoguiadas> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Calcula la disponibilidad de horarios de entrada para visitas autoguiadas.
        /// </summary>
        public Task<List<SlotDisponibleVisitaAutoguiada>> CalcularDisponibilidad(
            DateTime fechaDesde,
            DateTime fechaHasta,
            IReadOnlyCollection<VisitaGrupalAutoguiada> visitasAutoguiadas,
            ConfiguracionHorarioAutoguiada configuracion,
            HorarioAperturaMuseo calendario)
        {
            _logger.LogInformation(
                "Iniciando CalcularDisponibilidad. FechaDesde={FechaDesde}, FechaHasta={FechaHasta}",
                fechaDesde, fechaHasta);

            _logger.LogInformation(
                "Parámetros recibidos: VisitasNulas={VisitasNulas}, ConfiguracionNula={ConfiguracionNula}, CalendarioNulo={CalendarioNulo}",
                visitasAutoguiadas == null,
                configuracion == null,
                calendario == null);

            if (visitasAutoguiadas != null)
            {
                _logger.LogInformation(
                    "Cantidad de visitas autoguiadas recibidas: {Cantidad}",
                    visitasAutoguiadas.Count);
            }

            if (configuracion != null)
            {
                _logger.LogInformation(
                    "Configuración recibida: Id={Id}, CapacidadMaximaPorGrupo={CapacidadMaximaPorGrupo}, VisitasSimultaneasMaximas={VisitasSimultaneasMaximas}, DuracionVisita={DuracionVisita}, IntervaloReservas={IntervaloReservas}",
                    configuracion.Id,
                    configuracion.CapacidadMaximaPorGrupo,
                    configuracion.VisitasSimultaneasMaximas,
                    configuracion.DuracionVisita,
                    configuracion.IntervaloReservas);

                _logger.LogInformation(
                    "Capacidad máxima simultánea calculada: {Capacidad}",
                    configuracion.CalcularCapacidadMaximaSimultanea());
            }

            if (calendario != null)
            {
                _logger.LogInformation(
                    "Calendario recibido: Id={Id}, CantidadHorariosApertura={CantidadHorariosApertura}, CantidadDiasCierre={CantidadDiasCierre}",
                    calendario.Id,
                    calendario.HorariosApertura?.Count ?? 0,
                    calendario.DiasCierre?.Count ?? 0);
            }

            if (configuracion == null)
                throw new ArgumentNullException(nameof(configuracion));

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            if (visitasAutoguiadas == null)
                throw new ArgumentNullException(nameof(visitasAutoguiadas));

            var resultado = new List<SlotDisponibleVisitaAutoguiada>();

            for (DateTime fecha = fechaDesde.Date; fecha <= fechaHasta.Date; fecha = fecha.AddDays(1))
            {
                _logger.LogInformation(
                    "Procesando fecha {Fecha}.",
                    fecha.ToString("yyyy-MM-dd"));

                // 1. Verificar si el día está configurado como día laboral para autoguiadas.
                bool diaDisponible = configuracion.EstaDisponibleEnFecha(fecha);

                _logger.LogInformation(
                    "Fecha {Fecha}: EstaDisponibleEnFecha={Disponible}",
                    fecha.ToString("yyyy-MM-dd"),
                    diaDisponible);

                if (!diaDisponible)
                {
                    _logger.LogWarning(
                        "Se omite la fecha {Fecha} porque no está configurada como disponible.",
                        fecha.ToString("yyyy-MM-dd"));

                    continue;
                }

                // CORREGIDO: Convertimos a DateOnly para pasarlo como argumento requerido
                var fechaDateOnly = DateOnly.FromDateTime(fecha);

                // 2. CORREGIDO: Pasamos fechaDateOnly a GenerarSlotsDisponibles
                var slots = configuracion
                    .GenerarSlotsDisponibles(fechaDateOnly)
                    .ToList();

                _logger.LogInformation(
                    "Fecha {Fecha}: se generaron {CantidadSlots} slots.",
                    fecha.ToString("yyyy-MM-dd"),
                    slots.Count);

                foreach (var slotGenerado in slots)
                {
                    _logger.LogInformation(
                        "Slot generado: Inicio={Inicio}, Fin={Fin}",
                        slotGenerado.Inicio,
                        slotGenerado.Fin);
                }

                // Si no existen slots, no hay nada que procesar.
                if (slots.Count == 0)
                {
                    _logger.LogWarning(
                        "No se generaron slots para la fecha {Fecha}.",
                        fecha.ToString("yyyy-MM-dd"));

                    continue;
                }

                // 3. Verificar si el museo está operativo a nivel de calendario general.
                // Como EsDiaOperativo evalúa a las 00:00:00 (medianoche), si no coincide con
                // tu hora de apertura, los slots individuales se validarán de forma segura en el bucle inferior.
                bool diaOperativo = calendario.EsDiaOperativo(fecha);

                _logger.LogInformation(
                    "Fecha {Fecha}: EsDiaOperativo={DiaOperativo}",
                    fecha.ToString("yyyy-MM-dd"),
                    diaOperativo);

                if (!diaOperativo)
                {
                    _logger.LogWarning(
                        "El calendario indica que la fecha {Fecha} no está operativa. Se marcarán sus slots como NoDisponible.",
                        fecha.ToString("yyyy-MM-dd"));

                    foreach (var slot in slots)
                    {
                        _logger.LogInformation(
                            "Slot {Inicio}-{Fin}: Estado=NoDisponible por día no operativo.",
                            slot.Inicio,
                            slot.Fin);

                        resultado.Add(
                            new SlotDisponibleVisitaAutoguiada(
                                slot,
                                configuracion.CalcularCapacidadMaximaSimultanea(),
                                0,
                                configuracion.CapacidadMaximaPorGrupo,
                                EstadoTurno.NoDisponible));
                    }

                    continue;
                }

                // 4. Procesar cada horario posible de entrada.
                foreach (var slot in slots)
                {
                    _logger.LogInformation(
                        "Validando slot: Inicio={Inicio}, Fin={Fin}",
                        slot.Inicio,
                        slot.Fin);

                    // 4.1. Verificar que el horario completo esté dentro del horario operativo del museo.
                    bool estaAbierto = calendario.EstaAbierto(slot.Inicio, slot.Fin);

                    _logger.LogInformation(
                        "Slot {Inicio}-{Fin}: EstaAbierto={EstaAbierto}",
                        slot.Inicio,
                        slot.Fin,
                        estaAbierto);

                    if (!estaAbierto)
                    {
                        _logger.LogWarning(
                            "Slot {Inicio}-{Fin} fuera del horario operativo. Estado=NoDisponible.",
                            slot.Inicio,
                            slot.Fin);

                        resultado.Add(
                            new SlotDisponibleVisitaAutoguiada(
                                slot,
                                configuracion.CalcularCapacidadMaximaSimultanea(),
                                0,
                                configuracion.CapacidadMaximaPorGrupo,
                                EstadoTurno.NoDisponible));

                        continue;
                    }

                    // 4.2. Determinar cuántas visitas existentes se encuentran simultáneamente con el nuevo grupo.
                    int visitasSimultaneas = ContarVisitasSimultaneas(slot, visitasAutoguiadas);

                    _logger.LogInformation(
                        "Slot {Inicio}-{Fin}: visitas simultáneas encontradas={Cantidad}.",
                        slot.Inicio,
                        slot.Fin,
                        visitasSimultaneas);

                    // 4.3. Calcular cuántos grupos adicionales pueden entrar.
                    int cuposDisponibles = Math.Max(0, configuracion.VisitasSimultaneasMaximas - visitasSimultaneas);

                    _logger.LogInformation(
                        "Slot {Inicio}-{Fin}: máximo simultáneo={Maximo}, ocupados={Ocupados}, cupos disponibles={Disponibles}.",
                        slot.Inicio,
                        slot.Fin,
                        configuracion.VisitasSimultaneasMaximas,
                        visitasSimultaneas,
                        cuposDisponibles);

                    // 4.4. Estado del horario.
                    EstadoTurno estado = cuposDisponibles <= 0 ? EstadoTurno.Completo : EstadoTurno.Disponible;

                    _logger.LogInformation(
                        "Slot {Inicio}-{Fin}: estado final={Estado}.",
                        slot.Inicio,
                        slot.Fin,
                        estado);

                    // 4.5. Agregar resultado.
                    resultado.Add(
                        new SlotDisponibleVisitaAutoguiada(
                            slot,
                            configuracion.CalcularCapacidadMaximaSimultanea(),
                            cuposDisponibles,
                            configuracion.CapacidadMaximaPorGrupo,
                            estado));
                }
            }

            _logger.LogInformation(
                "CalcularDisponibilidad finalizado. Total de slots devueltos={CantidadResultados}.",
                resultado.Count);

            return Task.FromResult(resultado);
        }

        /// <summary>
        /// Cuenta las visitas existentes que se solapan con el período completo de permanencia del nuevo grupo.
        /// </summary>
        private int ContarVisitasSimultaneas(
            TimeSlot slot,
            IReadOnlyCollection<VisitaGrupalAutoguiada> visitasAutoguiadas)
        {
            _logger.LogInformation(
                "Contando visitas simultáneas para el slot {Inicio}-{Fin}. Total de visitas a evaluar={CantidadVisitas}.",
                slot.Inicio,
                slot.Fin,
                visitasAutoguiadas.Count);

            int contador = 0;

            foreach (var visita in visitasAutoguiadas)
            {
                bool seSolapa = visita.Horario.SeSolapaCon(slot);

                _logger.LogInformation(
                    "Visita evaluada: VisitaId={VisitaId}, Horario={Horario}, Slot={InicioSlot}-{FinSlot}, SeSolapa={SeSolapa}.",
                    visita.Id,
                    visita.Horario,
                    slot.Inicio,
                    slot.Fin,
                    seSolapa);

                if (seSolapa)
                    contador++;
            }

            _logger.LogInformation(
                "Resultado del conteo para el slot {Inicio}-{Fin}: {CantidadSimultaneas}.",
                slot.Inicio,
                slot.Fin,
                contador);

            return contador;
        }
    }
}
