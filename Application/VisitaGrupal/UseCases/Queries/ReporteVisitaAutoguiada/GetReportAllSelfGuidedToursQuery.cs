
using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.ReporteVisitaAutoguiada{
    
    public class GetReportAllSelfGuidedToursQuery(DateTime desde, DateTime hasta) : QueryRequest<QueryResult<VisitaAutoguiadaDto>>
    {
        public DateTime Desde { get; } = desde;
        public DateTime Hasta { get; } = hasta;

                
    }

}
