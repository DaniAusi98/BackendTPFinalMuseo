using Application.VisitaGrupal.DataTransferObjets;

using Core.Application;

namespace Application.VisitaGrupal.UseCases.Tematicas.Queries.GetTematicaBy
{
    public class GetTematicaByQuery : QueryRequest<TematicaVisitaDto>
    {
        public int TematicaId { get; set; }
    }
}
