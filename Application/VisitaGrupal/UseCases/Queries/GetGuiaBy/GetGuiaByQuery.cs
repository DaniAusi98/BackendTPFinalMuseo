using Application.VisitaGrupal.DataTransferObjets.Guias;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetGuiaBy
{
    public class GetGuiaByQuery : QueryRequest<GuiaDto>
    {
        public string GuiaId { get; set; }
    }
}
