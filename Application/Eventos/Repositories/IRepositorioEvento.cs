using Core.Application.Repositories;
using Domain.Eventos.Entities;

namespace Application.Eventos.Repositories
{
    public interface IRepositorioEvento : IRepository<Evento>
    {
        public Task<List<Evento>> FindAllAsync(DateTime fechaDesde, DateTime fechaHasta);

    }
}