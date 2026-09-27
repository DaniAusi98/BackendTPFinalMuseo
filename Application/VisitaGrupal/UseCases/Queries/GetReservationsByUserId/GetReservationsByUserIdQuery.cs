using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetReservationsByUserId
{
    public class GetReservationsByUserIdQuery : QueryRequest<QueryResult<GuidedTourReservationDto>>
    {
        public string UsuarioVisitanteId { get; set; }
        public GetReservationsByUserIdQuery()
        {
                
        }
    }
}
