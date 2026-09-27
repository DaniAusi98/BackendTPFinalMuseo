using Core.Application.Repositories;
using Domain.Common.Entities;

namespace Application.Common.Repositories
{
    public interface IDepartamentoRepository : IRepository<Departamento>
    {
        Task<List<Departamento>> GetDepartamentoByProvinciaIdAsync(string provinciaId,CancellationToken cancellationToken = default);
        Task<List<Departamento>> GetByIdsAsync(IEnumerable<string> ids);

    }
}
