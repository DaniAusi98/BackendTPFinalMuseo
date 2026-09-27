using System;
using System.Threading.Tasks;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;

namespace Application.VisitaGrupal.UseCases.Queries.GetAllGuias
{
    internal class GetAllGuiasHandler(IRepositorioGuia repositorioGuia) : IRequestQueryHandler<GetAllGuiasQuery, QueryResult<GuiaDto>>
    {
        private readonly IRepositorioGuia _repositorioGuia = repositorioGuia ?? throw new ArgumentNullException(nameof(repositorioGuia));
        public async Task<QueryResult<GuiaDto>> Handle(GetAllGuiasQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.RecursoMuseo.Entities.Guia.Guia> entities = await _repositorioGuia.FindAllAsync();
            return new QueryResult<GuiaDto>(entities.To<GuiaDto>(), entities.Count, request.PageIndex, request.PageSize);
        }
    }
}
