using Application.Eventos.DataTransferObjets;
using Application.Eventos.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.Eventos.Entities;

namespace Application.Eventos.UseCases.Queries.GetAllPropuestas
{
    internal sealed class GetAllPropuestaEventoHandler(
        IRepositorioPropuestaEvento repositorioPropuestaEvento) :
        IRequestQueryHandler<GetAllPropuestasEventoQuery, QueryResult<PropuestaEventoDto>>
    {
        private readonly IRepositorioPropuestaEvento repositorioPropuestaEvento1 = repositorioPropuestaEvento ?? throw new ArgumentNullException(nameof(repositorioPropuestaEvento));
        public async Task<QueryResult<PropuestaEventoDto>> Handle(GetAllPropuestasEventoQuery request, CancellationToken cancellationToken)
        {
            List<PropuestaEvento> propuestaEventos = await repositorioPropuestaEvento1.FindIncludeAllAsync();
            return new QueryResult<PropuestaEventoDto>(propuestaEventos.To<PropuestaEventoDto>(), propuestaEventos.Count, request.PageIndex, request.PageSize);

        }
    }
}
