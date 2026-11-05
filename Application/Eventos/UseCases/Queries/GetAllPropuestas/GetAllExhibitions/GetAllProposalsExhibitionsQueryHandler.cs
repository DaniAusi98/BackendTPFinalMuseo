using Application.Eventos.DataTransferObjets;
using Application.Eventos.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.Eventos.Entities;

namespace Application.Eventos.UseCases.Queries.GetAllPropuestas.GetAllExhibitions
{
    internal sealed class GetAllExhibitionsQueryHandler(IRepositorioMuestraTemporal repositorioMuestraTemporal,
        IRepositorioPropuestaMuestra repositorioPorpuestaMuestra) : IRequestQueryHandler<GetAllProposalsExhibitionsQuery, QueryResult<DataTransferObjets.PropuestaMuestraDto>>
    {
        private readonly IRepositorioMuestraTemporal _repositorioMuestraTemporal = repositorioMuestraTemporal ?? throw new ArgumentNullException(nameof(repositorioMuestraTemporal));
        private readonly IRepositorioPropuestaMuestra __repositorioPropuestaMuestra = repositorioPorpuestaMuestra ?? throw new ArgumentNullException(nameof(repositorioPorpuestaMuestra));

        public async Task<QueryResult<PropuestaMuestraDto>> Handle(GetAllProposalsExhibitionsQuery request, CancellationToken cancellationToken)
        {
            List<PropuestaMuestra> propuestaMuestras = await __repositorioPropuestaMuestra.FindIncludeAllAsync();

            return new QueryResult<PropuestaMuestraDto>(propuestaMuestras.To<PropuestaMuestraDto>(), propuestaMuestras.Count, request.PageIndex, request.PageSize);

        }


    }
}
