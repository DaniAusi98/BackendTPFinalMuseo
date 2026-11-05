using Application.MuseumResources.DataTransferObjects;
using Application.MuseumResources.Repositories;
using AutoMapper;
using Core.Application;
using Core.Application.Mapping;
using Microsoft.Extensions.Logging;

namespace Application.ActividadesEducativas.UseCases.Queries.SalasDispActEducativas
{
    internal class ObtenerSalasActEducHandler(IRepositorioSala repositorioSala,
        IRepositorioConfiguracionSalaActividad repositorioConfiguracion,
        IMapper mapper,
        ILogger<ObtenerSalasActEducHandler> logger)
        : IRequestQueryHandler<ObtenerSalasParaActEducQuery, QueryResult<SalaDto>>
    {
        private readonly IRepositorioSala _repositorioSala =
       repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));
        private readonly ILogger<ObtenerSalasActEducHandler> _logger = logger;

        private readonly IRepositorioConfiguracionSalaActividad _repositorioConfiguracion =
            repositorioConfiguracion ?? throw new ArgumentNullException(nameof(repositorioConfiguracion));

        private readonly IMapper _mapper =
            mapper ?? throw new ArgumentNullException(nameof(mapper));
        public async Task<QueryResult<SalaDto>> Handle(
    ObtenerSalasParaActEducQuery request,
    CancellationToken cancellationToken)
        {

            var salasActivas = await _repositorioSala.ObtenerSalasEventosActividadesAsync();
            return new QueryResult<SalaDto>(salasActivas.To<SalaDto>(), salasActivas.Count, request.PageIndex, request.PageSize);
        }
    }
}
