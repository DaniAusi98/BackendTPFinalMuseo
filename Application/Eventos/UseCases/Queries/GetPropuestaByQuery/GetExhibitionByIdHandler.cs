using Application.Eventos.DataTransferObjets;
using Application.Eventos.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.Eventos.Entities;

namespace Application.Eventos.UseCases.Queries.GetPropuestaByQuery
{
    internal sealed class GetExhibitionByIdHandler(IRepositorioPropuestaMuestra repositorioPropuestaMuestra) : IRequestQueryHandler<GetExhibitionByIdQuery, PropuestaMuestraDto>
    {
        private readonly IRepositorioPropuestaMuestra _repositorioPropuestaMuestra = repositorioPropuestaMuestra ?? throw new ArgumentNullException(nameof(repositorioPropuestaMuestra));
        public async Task<PropuestaMuestraDto> Handle(GetExhibitionByIdQuery request, CancellationToken cancellationToken)
        {
            PropuestaMuestra propuestaMuestra = await _repositorioPropuestaMuestra.FindIncludeByIdAsync(request.PropuestaId);
            return propuestaMuestra.To<PropuestaMuestraDto>();
        }
    }
}
