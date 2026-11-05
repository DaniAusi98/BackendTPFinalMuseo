using Core.Application.Repositories;
using Domain.Eventos.Entities;

namespace Application.Eventos.Repositories
{
    public interface IRepositorioMuestraTemporal : IRepository<MuestraTemporal>
    {
        public Task<List<MuestraTemporal>> FindAllIncludeAsync();


        public Task<MuestraTemporal?> FindOneIncludeAsync(string muestraId);

    }
}
