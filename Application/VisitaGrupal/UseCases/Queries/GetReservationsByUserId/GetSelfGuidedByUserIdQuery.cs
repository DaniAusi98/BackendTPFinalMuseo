using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;


namespace Application.VisitaGrupal.UseCases.Queries.GetReservationsByUserId
{
    public class GetSelfGuidedByUserIdQuery : QueryRequest<QueryResult<VisitaAutoguiadaDto>>
    {
        public string UsuarioVisitanteId { get; set; } = default!;

        public GetSelfGuidedByUserIdQuery()
        {
                
        }
    }
}
