using Domain.VisitasGrupales.Entities.GrupalGuiada;

public class ReglaMaximoVisitasSimultaneas
    : IReglaDisponibilidadVisitaGuiada
{
    public ResultadoReglaDisponibilidad Evaluar(
        ContextoDisponibilidadVisitaGuiada contexto)
    {
        int reservasSimultaneas = contexto.VisitasExistentes.Count;

        if (reservasSimultaneas >=
            contexto.Configuracion.MaximoVisitasSimultaneas)
        {
            return ResultadoReglaDisponibilidad.Invalida(
                "Se alcanzó el máximo de reservas simultáneas para este turno.");
        }

        return ResultadoReglaDisponibilidad.Valida();
    }
}