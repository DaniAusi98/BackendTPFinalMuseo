using Application.MuseumResources.DataTransferObjects;

namespace Application.MuseumResources.ApplicationServices
{
    public interface IDisponibilidadRecursosService
    {
        Task<List<DisponibilidadRecursoDto>> ConsultarAsync(
            DateTime inicio,
            DateTime fin,
            string? rrule,
            string? eventoIdExcluir);
    }
}