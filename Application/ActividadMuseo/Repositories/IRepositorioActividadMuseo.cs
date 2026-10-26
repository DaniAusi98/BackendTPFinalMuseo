using Core.Application.Repositories;

namespace Application.ActividadMuseo.Repositories
{
    public interface IRepositorioActividadMuseo : IRepository<Domain.ActividadMuseo.Entities.ActividadBase>
    {
        public Task<List<Domain.ActividadMuseo.Entities.ActividadBase>> FindAllAsync(DateTime fechaDesde, DateTime fechaHasta);
        public Task<List<Domain.ActividadMuseo.Entities.ActividadBase>> FindAllCalendarAsync(DateTime fechaDesde, DateTime fechaHasta);

    }
}
