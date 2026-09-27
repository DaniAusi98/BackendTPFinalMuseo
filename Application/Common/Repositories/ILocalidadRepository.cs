using Core.Application.Repositories;
using Domain.Common.Entities;

namespace Application.Common.Repositories
{
    public interface ILocalidadRepository : IRepository<LocalidadArg>
    {
        Task<List<LocalidadArg>> GetByDepartamentoIdAsync(string departamentoId,CancellationToken cancellationToken = default);
        Task<List<LocalidadArg>> GetByIdsAsync(IEnumerable<string> ids);

    }
}
