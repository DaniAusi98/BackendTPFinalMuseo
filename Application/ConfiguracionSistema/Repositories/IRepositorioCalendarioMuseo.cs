using Core.Application.Repositories;
using Domain.Common.Entities;

namespace Application.ConfiguracionSistema.Repositories
{
    public interface IRepositorioCalendarioMuseo : IRepository<HorarioAperturaMuseo>
    {
        Task<HorarioAperturaMuseo?> ObtenerCalendarioActivoAsync();
    }
}
