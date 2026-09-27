using Application.MuseumResources.ApplicationServices;
using Application.MuseumResources.DataTransferObjects;
using Core.Application;

namespace Application.MuseumResources.UseCases.Recurso.Queries.DisponibilidadRecursos
{
    internal sealed class GetDisponibilidadRecursosPorActividadHandler(
        IDisponibilidadRecursosService disponibilidadRecursosService)
        : IRequestQueryHandler<
            GetDisponibilidadRecursosPorActividadQuery,
            List<DisponibilidadRecursoDto>>
    {
        private readonly IDisponibilidadRecursosService _disponibilidadRecursosService =
            disponibilidadRecursosService
            ?? throw new ArgumentNullException(nameof(disponibilidadRecursosService));

        public async Task<List<DisponibilidadRecursoDto>> Handle(
            GetDisponibilidadRecursosPorActividadQuery request,
            CancellationToken cancellationToken)
        {
            return await _disponibilidadRecursosService.ConsultarAsync(
                request.Inicio,
                request.Fin,
                request.RRule);
        }
    }
}