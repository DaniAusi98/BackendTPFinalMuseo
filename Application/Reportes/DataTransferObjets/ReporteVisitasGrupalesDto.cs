using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Application.Reportes.DataTransferObjets
{
    public class ReporteVisitasGrupalesDto
    {
        public string Id { get; set; }
        public int ReservasTotales { get; private set; }
        public int VisitanteTotales { get; private set; }
        public int VisitasConfirmadas { get; private set; }
        public int VisitasCanceladas { get; private set; }
        public int Reprogramadas { get; private set; }
        public int Pendientes { get; private set; }

        public ReporteVisitasGrupalesDto(
            int reservasTotales,
            int visitanteTotales,
            int visitasConfirmadas,
            int visitasCanceladas,
            int reprogramadas,
            int pendientes)
        {
            ReservasTotales = reservasTotales;
            VisitanteTotales = visitanteTotales;
            VisitasConfirmadas = visitasConfirmadas;
            VisitasCanceladas = visitasCanceladas;
            Reprogramadas = reprogramadas;
            Pendientes = pendientes;
        }
    }
}