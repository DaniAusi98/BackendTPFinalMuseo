using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetGuiaBy
{
    public class GetGuiaByQuery : QueryRequest<DataTransferObjets.GuiaDto>
    {
        public int GuiaId { get; set; }
    }
}
