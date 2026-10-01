using Application.MuseumResources.Repositories;
using AutoMapper;
using Core.Application;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.RecursoMuseo.Enums.Enums;

namespace Application.Eventos.UseCases.Queries.SalasDisponibles
{
    internal sealed class ObtenerSalasParaEventoQueryHandler(
        IRepositorioSala repositorioSala,
        IRepositorioConfiguracionSalaActividad repositorioConfiguracion,
        IMapper mapper)
        : IRequestQueryHandler<SalaDispEventosQuery, QueryResult<SalaDisponibleParaEventoDto>>
    {
        private readonly IRepositorioSala _repositorioSala = 
            repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));

        private readonly IRepositorioConfiguracionSalaActividad _repositorioConfiguracion =
            repositorioConfiguracion ?? throw new ArgumentNullException(nameof(repositorioConfiguracion));

        private readonly IMapper _mapper = 
            mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<QueryResult<SalaDisponibleParaEventoDto>> Handle(
            SalaDispEventosQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Obtener todas las salas activas
            var salasActivas = await _repositorioSala.ObtenerSalasDisponiblesAsync();

            // 2. Obtener configuraciones habilitadas para Evento
            var configuracionesEvento = await _repositorioConfiguracion
                .ObtenerPorTipoActividadAsync(TipoActividad.Evento);

            // 3. Filtrar: salas activas que tienen configuración habilitada para Evento
            var salasParaEventos = salasActivas
                .Where(s => s.EstadoSala == EstadoSala.Activa &&
                            configuracionesEvento.Any(config => 
                                config.SalaId == s.Id && 
                                config.Habilitada))
                .ToList();

            // 4. Mapear a DTOs
            var dtos = salasParaEventos
                .Select(s => new SalaDisponibleParaEventoDto
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    CodigoSala = s.CodigoSala,
                    Capacidad = s.Capacidad,
                    Ubicacion = s.Ubicacion.ToString(),
                    TipoSala = s.TipoSala.ToString()
                })
                .ToList();

            return new QueryResult<SalaDisponibleParaEventoDto>(
                dtos,
                dtos.Count,
                pageIndex: 0,
                pageSize: (uint)dtos.Count);
        }
    }
}
