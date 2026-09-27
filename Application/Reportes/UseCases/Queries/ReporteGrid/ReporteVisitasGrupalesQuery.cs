using Application.Reportes.DataTransferObjets;
using Core.Application;

namespace Application.Reportes.UseCases.Queries.ReporteGrid
{
    public class ReporteVisitasGrupalesQuery:QueryRequest<QueryResult<ReporteGridDto>>
    {
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
    }
    
}
