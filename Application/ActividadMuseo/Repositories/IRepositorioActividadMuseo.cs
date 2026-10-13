using Core.Application.Repositories;

namespace Application.ActividadMuseo.Repositories
{
    public interface IRepositorioActividadMuseo : IRepository<Domain.ActividadMuseo.Entities.ActividadMuseo>
    {
        public Task<List<Domain.ActividadMuseo.Entities.ActividadMuseo>> FindAllAsync(DateTime fechaDesde, DateTime fechaHasta);
        public Task<List<Domain.ActividadMuseo.Entities.ActividadMuseo>> FindAllCalendarAsync(DateTime fechaDesde, DateTime fechaHasta);

    }
}
