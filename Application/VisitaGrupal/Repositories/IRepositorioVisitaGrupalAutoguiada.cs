
using Core.Application.Repositories;
using Domain.VisitasGrupales.Entities.GrupalAutoguiada;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioVisitaGrupalAutoguiada : IRepository<VisitaGrupalAutoguiada>
    {
        Task<List<VisitaGrupalAutoguiada>> GetAllGroupVisitAuAsync(DateTime fechaDesde, DateTime fechaHasta);
        Task<List<VisitaGrupalAutoguiada>> GetActiveSelfGuidedTours(
           DateTime fechaDesde,
           DateTime fechaHasta);

        Task<List<VisitaGrupalAutoguiada>> GetAllGroupVisitCalendarAsync(DateTime fechaDesde, DateTime fechaHasta);

        Task<List<VisitaGrupalAutoguiada>> ObtenerPorUsuarioIdAsync(string usuarioId, DateTime fechaActual);

        Task<VisitaGrupalAutoguiada?> FindByIdWithActividadAsync(string id);


    }
}
