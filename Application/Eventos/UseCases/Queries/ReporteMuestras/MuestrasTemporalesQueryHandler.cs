using Application.Eventos.DataTransferObjets;
using Application.Eventos.Repositories;
using Core.Application;
using Core.Application.Mapping;

namespace Application.Eventos.UseCases.Queries.ReporteMuestras
{
    internal sealed class MuestrasTemporalesQueryHandler(
        IRepositorioMuestraTemporal repositorioMuestraTemporal)
        : IRequestQueryHandler<MuestrasTemporalesQuery, QueryResult<MuestraDto>>
    {
        private readonly IRepositorioMuestraTemporal _repositorioMuestraTemporal =
            repositorioMuestraTemporal ?? throw new ArgumentNullException(nameof(repositorioMuestraTemporal));

        public async Task<QueryResult<MuestraDto>> Handle(
            MuestrasTemporalesQuery request,
            CancellationToken cancellationToken)
        {
            var fechaDesde = request.Desde.Date;

            var fechaHasta = (request.Hasta ?? request.Desde)
                .Date
                .AddDays(1)
                .AddSeconds(-1);

            var muestras = await _repositorioMuestraTemporal.FindAllIncludeByPeriodAsync(
                fechaDesde,
                fechaHasta);

            var resultado = muestras
                .Select(x => x.To<MuestraDto>())
                .ToList();

            return new QueryResult<MuestraDto>(
            resultado,
            resultado.Count,
            request.PageIndex,
            request.PageSize);
        }
    }
}