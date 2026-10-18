namespace Domain.VisitasGrupales.Entities.GrupalGuiada.ReglasDisponibilidad;

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