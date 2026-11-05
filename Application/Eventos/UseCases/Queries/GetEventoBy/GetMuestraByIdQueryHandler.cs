using Application.Eventos.DataTransferObjets;
using Application.Eventos.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.Eventos.Entities;

namespace Application.Eventos.UseCases.Queries.GetEventoBy
{
    internal class GetMuestraByIdQueryHandler(IRepositorioMuestraTemporal repositorioMuestraTemporal) : IRequestQueryHandler<GetMuestraByIdQuery, MuestraDto>
    {
        private readonly IRepositorioMuestraTemporal _repositorioMuestraTemporal = repositorioMuestraTemporal ?? throw new ArgumentNullException(nameof(repositorioMuestraTemporal));
        public async Task<MuestraDto> Handle(GetMuestraByIdQuery request, CancellationToken cancellationToken)
        {
            MuestraTemporal muestraTemporal = await _repositorioMuestraTemporal.FindOneIncludeAsync(request.Id);
            return muestraTemporal.To<MuestraDto>();
        }
    }
}
