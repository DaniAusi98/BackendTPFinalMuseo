using Application.PersonalMuseo.DataTrasnferObjets;
using Application.PersonalMuseo.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.PersonalMuseo.Entities.UsuarioInterno;
using MediatR;

namespace Application.PersonalMuseo.UseCases.Queries.ObtenerPuestosArea
{
    internal class ObtenerPuestoAreaHandler(IRepositorioAreaPuesto repositorioAreaPuesto) : IRequestQueryHandler<ObtenerPuestoAreaQuery, QueryResult<AreaPuestoDto>>
    {
        private readonly IRepositorioAreaPuesto _repositorioAreaPuesto = repositorioAreaPuesto;


        async Task<QueryResult<AreaPuestoDto>> IRequestHandler<ObtenerPuestoAreaQuery, QueryResult<AreaPuestoDto>>.Handle(ObtenerPuestoAreaQuery request, CancellationToken cancellationToken)
        {
            List<AreaPuesto> puestosArea = await _repositorioAreaPuesto.ObtenerPuestosPorArea(request.AreaId);

            return new QueryResult<AreaPuestoDto>(puestosArea.To<AreaPuestoDto>(), puestosArea.Count, request.PageIndex, request.PageSize);
        }
    }
}
