using Core.Application.Repositories;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioVisitaGuiada : IRepository<VisitaGrupalGuiada>
    {
        //Task<List<VisitaGrupalGuiada>> FindByUserIdAsync(Expression<Func<VisitaGrupalGuiada, bool>> filter);

        Task<List<VisitaGrupalGuiada>> GetAllGroupVisitAsync(DateTime fechaDesde, DateTime fechaHasta);

        Task<List<VisitaGrupalGuiada>> GetAllGroupVisitCalendarAsync(DateTime fechaDesde, DateTime fechaHasta);

        Task<List<VisitaGrupalGuiada>> ObtenerPorUsuarioIdAsync(string usuarioId, DateTime fechaActual);
        Task<VisitaGrupalGuiada> FindByIdWithActividadAsync(string id);



    }
}
