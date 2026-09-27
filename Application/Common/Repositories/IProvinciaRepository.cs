using Core.Application.Repositories;
using Domain.Common.Entities;

namespace Application.Common.Repositories
{
    public interface IProvinciaRepository : IRepository<Provincia>
    {
        Task<List<Provincia>> GetByIdsAsync(IEnumerable<string> ids);


    }
}
