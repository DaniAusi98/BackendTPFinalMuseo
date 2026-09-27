using Application.ApplicationMuseo.DataTransferObjects;
using Application.Common.Repositories;
using Core.Application;
using Core.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationMuseo.UseCases.Provincia.Queries.GetLocalidadesbyDepartamento
{
    internal sealed class GetLocalidadesByDepartamentoHanlder(ILocalidadRepository context) : IRequestQueryHandler<GetLocalidadesByDepartamentoQuery, QueryResult<LocalidadDto>>
    {
        public async Task<QueryResult<LocalidadDto>> Handle(GetLocalidadesByDepartamentoQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.Common.Entities.LocalidadArg> entities = await context.GetByDepartamentoIdAsync(request.DepartamentoId, cancellationToken);
            return new QueryResult<LocalidadDto>(entities.To<LocalidadDto>(), entities.Count, request.PageIndex, request.PageSize);
        }
    
    }
}
/*internal sealed class GetDepartamentosByProvinciaHandler(IDepartamentoRepository context) : IRequestQueryHandler<GetDepartamentosByProvinciaQuery, QueryResult<DepartamentoDto>>

    {
        public async Task<QueryResult<DepartamentoDto>> Handle(GetDepartamentosByProvinciaQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.Common.Entities.Departamento> entities = await context.GetDepartamentoByProvinciaIdAsync(request.ProvinciaId, cancellationToken);
            return new QueryResult<DepartamentoDto>(entities.To<DepartamentoDto>(), entities.Count, request.PageIndex, request.PageSize);
        }
    }
}*/