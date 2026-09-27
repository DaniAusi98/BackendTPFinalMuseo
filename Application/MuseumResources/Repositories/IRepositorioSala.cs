using Core.Application.Repositories;

using Domain.Common.Entities;
using Domain.RecursoMuseo.Entities;

namespace Application.MuseumResources.Repositories
{
    public interface IRepositorioSala: IRepository<Sala>
    {
        Task<List<Sala>> ObtenerSalasporIdsAsync(IEnumerable<string> ids);
        Task<List<Sala>> ObtenerSalasDisponiblesAsync();

    }
}
