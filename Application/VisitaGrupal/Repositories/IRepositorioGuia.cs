using Core.Application.Repositories;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioGuia : IRepository<Guia>
    {
        Task<List<Guia>> ObtenerGuiasConDisponibilidadAsync();
        Task<List<Guia>> ObtenerGuiasActivas();

        Task<Guia?> ObtenerGuiaConDisponibilidadAsync(string guiaId);
        Task<List<Guia>> ObtenerDisponibilidadGuias(
          DateTime desde,
          DateTime hasta);

        Task<Guia?> ObtenerGuiaPersonalId(string userId);




    }
}
