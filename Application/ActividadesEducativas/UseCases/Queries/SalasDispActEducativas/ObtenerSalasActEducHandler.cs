using Application.MuseumResources.Repositories;
using AutoMapper;
using Core.Application;
using Microsoft.Extensions.Logging;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.RecursoMuseo.Enums.Enums;

namespace Application.ActividadesEducativas.UseCases.Queries.SalasDispActEducativas
{
    internal class ObtenerSalasActEducHandler(IRepositorioSala repositorioSala,
        IRepositorioConfiguracionSalaActividad repositorioConfiguracion,
        IMapper mapper,
        ILogger<ObtenerSalasActEducHandler> logger)
        : IRequestQueryHandler<ObtenerSalasParaActEducQuery, QueryResult<SalasActEducDto>>
    {
        private readonly IRepositorioSala _repositorioSala =
       repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));
        private readonly ILogger<ObtenerSalasActEducHandler> _logger = logger;

        private readonly IRepositorioConfiguracionSalaActividad _repositorioConfiguracion =
            repositorioConfiguracion ?? throw new ArgumentNullException(nameof(repositorioConfiguracion));

        private readonly IMapper _mapper =
            mapper ?? throw new ArgumentNullException(nameof(mapper));
        public async Task<QueryResult<SalasActEducDto>> Handle(
    ObtenerSalasParaActEducQuery request,
    CancellationToken cancellationToken)
        {

            var salasActivas = await _repositorioSala.ObtenerSalasDisponiblesAsync();

            _logger.LogInformation(
                "SALAS: {@Salas}",
                salasActivas.Select(x => new
                {
                    x.Id,
                    x.Nombre,
                    x.EstadoSala
                }));

            var configuraciones = await _repositorioConfiguracion
                .ObtenerPorTipoActividadAsync(TipoActividad.ActividadEducativa);

            _logger.LogInformation(
                "CONFIGURACIONES: {@Configuraciones}",
                configuraciones.Select(x => new
                {
                    x.SalaId,
                    x.TipoActividad,
                    x.Habilitada
                }));

            var salasDisp = salasActivas
                .Where(s => s.EstadoSala == EstadoSala.Activa &&
                            configuraciones.Any(config =>
                                config.SalaId == s.Id &&
                                config.Habilitada))
                .ToList();

            _logger.LogInformation(
                "SALAS DISPONIBLES: {Cantidad}",
                salasDisp.Count);

            var dtos = salasDisp
                .Select(s => new SalasActEducDto
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    CodigoSala = s.CodigoSala,
                    Capacidad = s.Capacidad,
                    Ubicacion = s.Ubicacion.ToString(),
                    TipoSala = s.TipoSala.ToString()
                })
                .ToList();

            return new QueryResult<SalasActEducDto>(
                dtos,
                dtos.Count,
                pageIndex: 0,
                pageSize: (uint)dtos.Count);
        }
    }
}
