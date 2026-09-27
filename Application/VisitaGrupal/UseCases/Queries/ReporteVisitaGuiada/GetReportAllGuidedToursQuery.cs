
using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.ReporteVisitaGuiada{
    
    public class GetReportAllGuidedToursQuery(DateTime desde, DateTime hasta) : QueryRequest<QueryResult<GuidedTourReservationDto>>
    {
        public DateTime Desde { get; } = desde;
        public DateTime Hasta { get; } = hasta;

                
    }

}
