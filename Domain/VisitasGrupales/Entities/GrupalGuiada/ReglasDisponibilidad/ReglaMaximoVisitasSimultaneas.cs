using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Domain.VisitasGrupales.Services.Disponibilidad.Reglas;

public class ReglaMaximoVisitasSimultaneas
    : IReglaDisponibilidadVisitaGuiada
{
    public ResultadoReglaDisponibilidad Evaluar(
        ContextoDisponibilidadVisitaGuiada contexto)
    {
        int visitasSimultaneas =
            contexto.VisitasExistentes.Count;

        if (visitasSimultaneas >=
            contexto.Configuracion.MaximoVisitasSimultaneas)
        {
            return ResultadoReglaDisponibilidad.Invalida(
                "Se alcanzó el máximo de visitas simultáneas.");
        }

        return ResultadoReglaDisponibilidad.Valida();
    }
}