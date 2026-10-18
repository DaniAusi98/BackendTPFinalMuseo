
namespace Domain.VisitasGrupales.Entities.GrupalGuiada.ReglasDisponibilidad;

public class ReglaDisponibilidadGuias
    : IReglaDisponibilidadVisitaGuiada
{
    public ResultadoReglaDisponibilidad Evaluar(
        ContextoDisponibilidadVisitaGuiada contexto)
    {
        if (contexto.GuiasDisponibles <= 0)
        {
            return ResultadoReglaDisponibilidad.Invalida(
                "No quedan guías disponibles.");
        }

        return ResultadoReglaDisponibilidad.Valida();
    }
}