using Application.ApplicationMuseo.DataTransferObjects;
using Application.Common.Repositories;
using Core.Application;
using Core.Application.Mapping;


namespace Application.ApplicationMuseo.UseCases.Provincia.Queries.GetDepartamentosByProvincia
{
    internal sealed class GetDepartamentosByProvinciaHandler(IDepartamentoRepository context) : IRequestQueryHandler<GetDepartamentosByProvinciaQuery, QueryResult<DepartamentoDto>>

    {
        public async Task<QueryResult<DepartamentoDto>> Handle(GetDepartamentosByProvinciaQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.Common.Entities.Departamento> entities = await context.GetDepartamentoByProvinciaIdAsync(request.ProvinciaId, cancellationToken);
            return new QueryResult<DepartamentoDto>(entities.To<DepartamentoDto>(), entities.Count, request.PageIndex, request.PageSize);
        }
    }
}
