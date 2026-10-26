
namespace Domain.VisitasGrupales.Entities.GrupalGuiada.ReglasDisponibilidad;

public class ReglaCapacidadMaximaPorTurno
    : IReglaDisponibilidadVisitaGuiada
{
    public ResultadoReglaDisponibilidad Evaluar(
        ContextoDisponibilidadVisitaGuiada contexto)
    {
        if (contexto.CapacidadPersonasRestante <= 0)
        {
            return ResultadoReglaDisponibilidad.Invalida(
                "Se alcanzó la capacidad máxima de personas del turno.");
        }

        return ResultadoReglaDisponibilidad.Valida();
    }
}
