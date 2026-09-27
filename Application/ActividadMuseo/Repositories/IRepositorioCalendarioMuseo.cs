using Core.Application.Repositories;
using Domain.Common.Entities;

namespace Application.ActividadMuseo.Repositories
{
    public interface IRepositorioCalendarioMuseo : IRepository<CalendarioMuseo>
    {
        Task<CalendarioMuseo?> ObtenerCalendarioActivoAsync();
    }
}
