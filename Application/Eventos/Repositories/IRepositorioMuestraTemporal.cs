using Core.Application.Repositories;
using Domain.Eventos.Entities;

namespace Application.Eventos.Repositories
{
    public interface IRepositorioMuestraTemporal : IRepository<MuestraTemporal>
    {
        Task<List<MuestraTemporal>> FindAllIncludeAsync();

        Task<List<MuestraTemporal>> FindAllIncludeByPeriodAsync(
          DateTime fechaDesde,
          DateTime fechaHasta);


        Task<MuestraTemporal?> FindOneIncludeAsync(string muestraId);

    }
}
