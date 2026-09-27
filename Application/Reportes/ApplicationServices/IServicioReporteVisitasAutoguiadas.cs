using Application.Reportes.DataTransferObjets;
using Domain.VisitasGrupales.Entities;


namespace Application.Reportes.ApplicationServices
{
    public interface IServicioReporteVisitasAutoguiadas
    {
        ReporteVisitaAutoguiadaDto GenerarReporteVisitasAutoguiadas(
            IReadOnlyCollection<VisitaGrupalAutoguiada> visitasAutoguiadas,
            int totalCapacidadDisponible
            );
    }
}
