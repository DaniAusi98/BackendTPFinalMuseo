using Application.Reportes.DataTransferObjets;
using Domain.VisitasGrupales.Entities.GrupalGuiada;


namespace Application.Reportes.ApplicationServices
{
    public interface IServicioReporteVisitasGuiadas
    {
        ReporteVisitaGuiadaDto GenerarReporteGeneralVisitasGuiadas(
            IReadOnlyCollection<VisitaGrupalGuiada> visitaGrupalGuiadas,int capacidadTotalDisponible);
    }
}
