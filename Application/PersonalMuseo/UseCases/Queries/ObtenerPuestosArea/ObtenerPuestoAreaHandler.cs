using Application.PersonalMuseo.DataTrasnferObjets;
using Application.PersonalMuseo.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.PersonalMuseo.Entities.UsuarioInterno;

namespace Application.PersonalMuseo.UseCases.Queries.ObtenerPuestosArea
{
    internal class ObtenerPuestoAreaHandler(IRepositorioAreaPuesto repositorioAreaPuesto) : IRequestQueryHandler<ObtenerPuestoAreaQuery, QueryResult<PuestoDto>>
    {
        private readonly IRepositorioAreaPuesto _repositorioAreaPuesto = repositorioAreaPuesto;
        public async Task<QueryResult<PuestoDto>> Handle(ObtenerPuestoAreaQuery request, CancellationToken cancellationToken)
        {
            List<Puesto> puestos = await _repositorioAreaPuesto.ObtenerPuestosPorArea(request.AreaId);

            return new QueryResult<PuestoDto>(puestos.To<PuestoDto>(), puestos.Count, request.PageIndex, request.PageSize);



        }
    }
}
