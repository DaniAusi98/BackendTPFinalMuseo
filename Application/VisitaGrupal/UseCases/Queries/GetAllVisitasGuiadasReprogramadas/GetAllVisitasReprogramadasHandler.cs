using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Application.VisitaGrupal.UseCases.Queries.GetAllVisitasGuiadasReprogramadas
{
    internal sealed class GetAllVisitasReprogramadasHandler(IReporsitorioVisitaReprogramada reporsitorioVisitaReprogramada) : IRequestQueryHandler<GetAllVisitasReprogramadasQuery, QueryResult<VisitaGuiadaReprogramadaDto>>
    {
        private readonly IReporsitorioVisitaReprogramada _reporsitorioVisitaReprogramada = reporsitorioVisitaReprogramada ?? throw new ArgumentNullException(nameof(reporsitorioVisitaReprogramada));
        public async Task<QueryResult<VisitaGuiadaReprogramadaDto>> Handle(GetAllVisitasReprogramadasQuery request, CancellationToken cancellationToken)
        {
            IList<VisitaReprogramada> guiadasReprogramadas = await _reporsitorioVisitaReprogramada.FindAllAsync();
            return new QueryResult<VisitaGuiadaReprogramadaDto>(guiadasReprogramadas.To<VisitaGuiadaReprogramadaDto>(), guiadasReprogramadas.Count, request.PageIndex, request.PageSize);
        }
    }
}
