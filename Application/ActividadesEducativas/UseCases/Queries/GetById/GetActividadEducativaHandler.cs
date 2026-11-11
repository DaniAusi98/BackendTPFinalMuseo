using Application.ActividadesEducativas.DataTransferObjets;
using Application.ActividadesEducativas.Repositories;
using Core.Application;
using Core.Application.Mapping;

namespace Application.ActividadesEducativas.UseCases.Queries.GetById
{
    internal sealed class GetActividadEducativaHandler(IRepositorioActividadEducativa repositorioActividadEducativa) : IRequestQueryHandler<GetActividadEducativaQuery, ActividadEducativaDto>
    {
        public async Task<ActividadEducativaDto> Handle(GetActividadEducativaQuery request, CancellationToken cancellationToken)
        {
            var entity = await repositorioActividadEducativa.FindOneIncludeAsync(request.ActividadId);
            if (entity is null)
                throw new Exception($"No se encontró la actividad educativa con id {request.ActividadId}");
            return entity.To<ActividadEducativaDto>();
        }

    }
}
