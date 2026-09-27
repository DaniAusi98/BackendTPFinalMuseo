using Application.Reportes.DataTransferObjets;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using System.Collections.Generic;

namespace Application.Reportes.ApplicationServices
{
    public interface IServicioReporteVisitasGrupales
    {
        ReporteVisitasGrupalesDto GenerarReporteGeneralVisitasGrupales(
            IReadOnlyCollection<VisitaGrupalAutoguiada> visitaGrupalAutoguiadas,
            IReadOnlyCollection<VisitaGrupalGuiada> visitaGrupalGuiadas);
    }
}