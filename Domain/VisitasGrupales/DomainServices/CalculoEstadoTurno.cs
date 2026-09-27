using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.DomainServices
{
    internal static class CalculoEstadoTurno
    {
        public static EstadoTurno Calcular(
            bool tieneReserva,
            int cantidadGuias)
        {
            if (tieneReserva)
                return EstadoTurno.Completo;

            if (cantidadGuias > 0)
                return EstadoTurno.Disponible;

            return EstadoTurno.NoDisponible;
        }
    }
}