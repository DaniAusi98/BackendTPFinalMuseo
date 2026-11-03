using Application.Eventos.DataTransferObjets;
using Application.Eventos.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.Eventos.Entities;

namespace Application.Eventos.UseCases.Queries.GetPropuestaByQuery
{
    internal sealed class GetPropuestaByIdHandler(IRepositorioPropuestaEvento repositorioPropuestaEvento) : IRequestQueryHandler<GetPropuestaByIdQuery, PropuestaEventoDto>
    {
        private readonly IRepositorioPropuestaEvento _repositorioPropuestaEvento = repositorioPropuestaEvento ?? throw new ArgumentNullException(nameof(repositorioPropuestaEvento));
        public async Task<PropuestaEventoDto> Handle(GetPropuestaByIdQuery request, CancellationToken cancellationToken)
        {
            PropuestaEvento propuestaEvento = await _repositorioPropuestaEvento.FindIncludeByIdAsync(request.PropuestaId);
            return propuestaEvento.To<PropuestaEventoDto>();

        }
    }
}
