using Core.Application.Repositories;

using Domain.RecursoMuseo.Entities;

namespace Application.MuseumResources.Repositories
{
    public interface IRepositorioRecurso : IRepository<Recurso>
    {
        Task<List<Recurso>> FindActivosAsync();
    }
}
