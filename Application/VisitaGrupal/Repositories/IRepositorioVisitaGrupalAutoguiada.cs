
using Core.Application.Repositories;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioVisitaGrupalAutoguiada:IRepository<VisitaGrupalAutoguiada>
    {
        Task<List<VisitaGrupalAutoguiada>> GetAllGroupVisitAuAsync(DateTime fechaDesde, DateTime fechaHasta);
        Task<List<VisitaGrupalAutoguiada>> ObtenerPorUsuarioIdAsync(string usuarioId, DateTime fechaActual);


    }
}
