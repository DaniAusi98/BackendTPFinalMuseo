using Core.Application.Repositories;
using Domain.ActividadesAreaEducacion.Entities;
namespace Application.ActividadesEducativas.Repositories
{
    public interface IRepositorioActividadEducativa : IRepository<ActividadEducativa>
    {
        Task<List<ActividadEducativa>> GetAllActEducationAsync(DateTime fechaDesde, DateTime fechaHasta);

    }
}
