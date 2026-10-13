using Application.MuseumResources.Repositories;
using AutoMapper;
using Core.Application;
using Domain.RecursoMuseo.Entities;

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


            List<Sala> salasParaEventos = await _repositorioSala.ObtenerSalasEventoAsync();

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
