using Application.MuseumResources.DataTransferObjects;
using Core.Application;

namespace Application.MuseumResources.UseCases.Recurso.Queries.DisponibilidadRecursos
{
    public class GetDisponibilidadRecursosPorActividadQuery
        : QueryRequest<List<DisponibilidadRecursoDto>>
    {
        public DateTime Inicio { get; set; }

        public DateTime Fin { get; set; }

        public string RRule { get; set; }
    }
}