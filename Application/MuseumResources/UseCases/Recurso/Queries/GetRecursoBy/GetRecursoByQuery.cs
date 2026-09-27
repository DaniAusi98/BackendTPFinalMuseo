using Core.Application;

namespace Application.MuseumResources.UseCases.Recurso.Queries.GetRecursoBy
{
    public class GetRecursoByQuery : QueryRequest<Application.MuseumResources.DataTransferObjects.RecursoDto>
    {
        public int RecursoId { get; set; }
    }
}
