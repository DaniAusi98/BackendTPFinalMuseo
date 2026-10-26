
namespace Domain.VisitasGrupales.Entities.GrupalGuiada.ReglasDisponibilidad
{
    public class MotorDisponibilidadVisitasGuiadas
    {
        private readonly IReadOnlyCollection<IReglaDisponibilidadVisitaGuiada>
            _reglas;

        public MotorDisponibilidadVisitasGuiadas(
            IEnumerable<IReglaDisponibilidadVisitaGuiada> reglas)
        {
            _reglas = reglas.ToList().AsReadOnly();
        }

        public ResultadoDisponibilidad Evaluar(
            ContextoDisponibilidadVisitaGuiada contexto)
        {
            foreach (var regla in _reglas)
            {
                var resultado = regla.Evaluar(contexto);

                if (!resultado.EsValida)
                {
                    return ResultadoDisponibilidad.NoDisponible(
                        resultado.Motivo);
                }
            }

            // Capacidad que pueden cubrir los guías libres.
            int capacidadPorGuias =
                contexto.Configuracion.CalcularCapacidadDisponible(
                    contexto.GuiasDisponibles);

            // Respetar también la capacidad restante del turno.
            int cuposDisponibles = Math.Min(
                capacidadPorGuias,
                contexto.CapacidadPersonasRestante);

            if (cuposDisponibles <= 0)
            {
                return ResultadoDisponibilidad.NoDisponible(
                    "No quedan cupos disponibles para el turno.");
            }

            return ResultadoDisponibilidad.DisponibleCon(
                cuposDisponibles);
        }
    }
}
