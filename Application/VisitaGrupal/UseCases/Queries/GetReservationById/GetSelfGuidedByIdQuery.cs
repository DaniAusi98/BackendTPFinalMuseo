using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;


namespace Application.VisitaGrupal.UseCases.Queries.GetReservationById
{
    public class GetSelfGuidedByIdQuery:IRequestQuery<VisitaAutoguiadaDto>
    {
        public string VisitaId { get; set; }

        public GetSelfGuidedByIdQuery()
        {
                
        }
    }
}
