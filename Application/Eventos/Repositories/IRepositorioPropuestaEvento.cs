using Core.Application.Repositories;
using Domain.Eventos.Entities;

namespace Application.Eventos.Repositories
{
    public interface IRepositorioPropuestaEvento : IRepository<PropuestaEvento>
    {
        Task<List<PropuestaEvento>> FindIncludeAllAsync();
        Task<PropuestaEvento?> FindIncludeByIdAsync(string id);

        Task<PropuestaEvento?> FindByEventoIdAsync(string eventoId);


    }
}
