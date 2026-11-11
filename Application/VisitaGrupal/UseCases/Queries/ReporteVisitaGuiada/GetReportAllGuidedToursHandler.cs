using Application.Reportes.DataTransferObjets;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;

namespace Application.VisitaGrupal.UseCases.Queries.ReporteVisitaGuiada
{
    internal sealed class GetReportAllGuidedToursHandler(IRepositorioVisitaGuiada repositorioVisitaGuiada) : IRequestQueryHandler<GetReportAllGuidedToursQuery, QueryResult<VisitaGuiadaDto>>
    {

        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        public async Task<QueryResult<VisitaGuiadaDto>> Handle(GetReportAllGuidedToursQuery request, CancellationToken cancellationToken)
        {
            var visitasGuiadas= await _repositorioVisitaGuiada.GetAllGroupVisitAsync(request.Desde, request.Hasta);
            return new QueryResult<VisitaGuiadaDto>(visitasGuiadas.To<VisitaGuiadaDto>(), visitasGuiadas.Count, request.PageIndex, request.PageSize);
        }    
    }

}