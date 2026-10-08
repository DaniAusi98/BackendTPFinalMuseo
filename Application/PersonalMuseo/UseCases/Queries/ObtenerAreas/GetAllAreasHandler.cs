using Application.PersonalMuseo.DataTrasnferObjets;
using Application.PersonalMuseo.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.PersonalMuseo.Entities.UsuarioInterno;

namespace Application.PersonalMuseo.UseCases.Queries.ObtenerAreas
{
    internal sealed class GetAllAreasHandler(IRepositorioArea repositorioArea) : IRequestQueryHandler<ObtenerPuestoAreaQuery, QueryResult<AreaDto>>
    {
        private readonly IRepositorioArea _repositorioArea = repositorioArea ?? throw new ArgumentNullException(nameof(repositorioArea));
        public async Task<QueryResult<AreaDto>> Handle(ObtenerPuestoAreaQuery request, CancellationToken cancellationToken)
        {
            IList<Area> areas = await _repositorioArea.FindAllAsync();
            return new QueryResult<AreaDto>(areas.To<AreaDto>(), areas.Count, request.PageIndex, request.PageSize);


        }
    }
}
