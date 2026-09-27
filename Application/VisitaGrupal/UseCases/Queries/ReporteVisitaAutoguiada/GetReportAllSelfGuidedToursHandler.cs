using Application.Reportes.DataTransferObjets;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;

namespace Application.VisitaGrupal.UseCases.Queries.ReporteVisitaAutoguiada
{
    internal sealed class GetReportAllSelfGuidedToursHandler(IRepositorioVisitaGrupalAutoguiada repositorioVisitaAutoguiada) : IRequestQueryHandler<GetReportAllSelfGuidedToursQuery, QueryResult<VisitaAutoguiadaDto>>
    {

        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada = repositorioVisitaAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaAutoguiada));
        public async Task<QueryResult<VisitaAutoguiadaDto>> Handle(GetReportAllSelfGuidedToursQuery request, CancellationToken cancellationToken)
        {
            var visitasGuiadas= await _repositorioVisitaGrupalAutoguiada.GetAllGroupVisitAuAsync(request.Desde, request.Hasta);
            return new QueryResult<VisitaAutoguiadaDto>(visitasGuiadas.To<VisitaAutoguiadaDto>(), visitasGuiadas.Count, request.PageIndex, request.PageSize);
        }    
    }

}