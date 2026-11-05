using Core.Application.Repositories;
using Domain.Eventos.Entities;

namespace Application.Eventos.Repositories
{
    public interface IRepositorioPropuestaMuestra : IRepository<PropuestaMuestra>
    {
        Task<List<PropuestaMuestra>> FindIncludeAllAsync();
        Task<PropuestaMuestra?> FindIncludeByIdAsync(string id);

        Task<PropuestaMuestra?> FindByMuestraIdAsync(string MuestraId);


    }
}
