using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Application.VisitaGrupal.ApplicationServices
{
    public interface IVisitaGuiadaApplicationService
    {
        bool TurnoDisponible(VisitaGrupalGuiada value);

    }
}
