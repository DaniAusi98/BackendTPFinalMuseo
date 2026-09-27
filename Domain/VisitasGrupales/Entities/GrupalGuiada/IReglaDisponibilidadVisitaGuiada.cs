namespace Domain.VisitasGrupales.Entities.GrupalGuiada;

public interface IReglaDisponibilidadVisitaGuiada
{
    ResultadoReglaDisponibilidad Evaluar(
        ContextoDisponibilidadVisitaGuiada contexto);
}