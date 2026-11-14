
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

        public Task<List<SlotDisponibleVisitaAutoguiada>> CalcularDisponibilidad(
            DateTime fechaDesde,
            DateTime fechaHasta,
            IReadOnlyCollection<VisitaGrupalAutoguiada> visitasAutoguiadas,
            ConfiguracionHorarioAutoguiada configuracion,
            HorarioAperturaMuseo calendario)
        {
            if (configuracion == null)
                throw new ArgumentNullException(nameof(configuracion));

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            if (visitasAutoguiadas == null)
                throw new ArgumentNullException(nameof(visitasAutoguiadas));

            if (fechaDesde.Date > fechaHasta.Date)
                throw new ArgumentException(
                    "La fecha desde no puede ser posterior a la fecha hasta.");

            _logger.LogInformation(
                "Calculando disponibilidad de autoguiadas desde {Desde} hasta {Hasta}.",
                fechaDesde.Date,
                fechaHasta.Date);

            var resultado = new List<SlotDisponibleVisitaAutoguiada>();

            for (DateTime fecha = fechaDesde.Date;
                 fecha <= fechaHasta.Date;
                 fecha = fecha.AddDays(1))
            {
                _logger.LogInformation(
                    "Procesando fecha {Fecha}.",
                    fecha.ToString("yyyy-MM-dd"));

                // 1. Verificar si la fecha está habilitada para autoguiadas.
                bool diaDisponible = configuracion.EstaDisponibleEnFecha(fecha);

                if (!diaDisponible)
                {
                    _logger.LogInformation(
                        "Fecha {Fecha} no habilitada para autoguiadas.",
                        fecha.ToString("yyyy-MM-dd"));

                    continue;
                }

                // 2. Generar los turnos configurados para ese día.
                DateOnly fechaDateOnly = DateOnly.FromDateTime(fecha);

                var slots = configuracion
                    .GenerarSlotsDisponibles(fechaDateOnly)
                    .ToList();

                _logger.LogInformation(
                    "Fecha {Fecha}: se generaron {Cantidad} turnos.",
                    fecha.ToString("yyyy-MM-dd"),
                    slots.Count);

                // 3. Validar cada turno individualmente contra el calendario
                // general del museo. No se bloquea el día completo.
                foreach (var slot in slots)
                {
                    _logger.LogInformation(
                        "Validando turno {Inicio}-{Fin}.",
                        slot.Inicio,
                        slot.Fin);

                    bool estaAbierto = calendario.EstaAbierto(
                        slot.Inicio,
                        slot.Fin);

                    if (!estaAbierto)
                    {
                        _logger.LogInformation(
                            "Turno {Inicio}-{Fin} fuera del horario operativo.",
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

                    // 4. Contar las visitas que se solapan con el turno.
                    int visitasSimultaneas = ContarVisitasSimultaneas(
                        slot,
                        visitasAutoguiadas);

                    // 5. Verificar si ya existe una reserva con el mismo inicio.
                    bool horarioOcupado = visitasAutoguiadas.Any(
                        visita => visita.Horario.Inicio == slot.Inicio);

                    // 6. Calcular cuántas visitas simultáneas quedan disponibles.
                    int cuposDisponibles = Math.Max(
                        0,
                        configuracion.VisitasSimultaneasMaximas - visitasSimultaneas);

                    // Si ya existe una reserva con ese inicio, no ofrecer el turno.
                    if (horarioOcupado)
                    {
                        cuposDisponibles = 0;
                    }

                    EstadoTurno estado = cuposDisponibles > 0
                        ? EstadoTurno.Disponible
                        : EstadoTurno.Completo;



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
                "Disponibilidad calculada. Total de turnos devueltos: {Cantidad}.",
                resultado.Count);

            return Task.FromResult(resultado);
        }

        private static int ContarVisitasSimultaneas(
            TimeSlot slot,
            IReadOnlyCollection<VisitaGrupalAutoguiada> visitasAutoguiadas)
        {
            return visitasAutoguiadas.Count(
                visita => visita.Horario.SeSolapaCon(slot));
        }
        public Task<bool> ValidarDisponibilidadSlot(
    VisitaGrupalAutoguiada visita,
    IReadOnlyCollection<VisitaGrupalAutoguiada> visitasAutoguiadas,
    ConfiguracionHorarioAutoguiada configuracion,
    HorarioAperturaMuseo calendario)
        {
            ArgumentNullException.ThrowIfNull(visita);
            ArgumentNullException.ThrowIfNull(visitasAutoguiadas);
            ArgumentNullException.ThrowIfNull(configuracion);
            ArgumentNullException.ThrowIfNull(calendario);

            var slot = visita.Horario;

            // Verificar que la fecha esté habilitada.
            if (!configuracion.EstaDisponibleEnFecha(slot.Inicio))
                return Task.FromResult(false);

            // Verificar que el horario corresponda a un slot configurado.
            var slotsConfigurados = configuracion.GenerarSlotsDisponibles(
                DateOnly.FromDateTime(slot.Inicio));

            bool slotConfigurado = slotsConfigurados.Any(s =>
                s.Inicio == slot.Inicio &&
                s.Fin == slot.Fin);

            if (!slotConfigurado)
                return Task.FromResult(false);

            // Verificar que el museo esté abierto.
            if (!calendario.EstaAbierto(slot.Inicio, slot.Fin))
                return Task.FromResult(false);

            // Verificar que no exista otra visita en el mismo horario.
            bool horarioOcupado = visitasAutoguiadas.Any(v =>
                v.Horario.Inicio == slot.Inicio);

            if (horarioOcupado)
                return Task.FromResult(false);

            // Verificar cantidad máxima de visitas simultáneas.
            int visitasSimultaneas = ContarVisitasSimultaneas(
                slot,
                visitasAutoguiadas);

            int cuposDisponibles = Math.Max(
                0,
                configuracion.VisitasSimultaneasMaximas - visitasSimultaneas);

            return Task.FromResult(cuposDisponibles > 0);
        }
    }
}
