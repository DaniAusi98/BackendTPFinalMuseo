using Core.Application.Repositories;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioGuia : IRepository<Guia>
    {
        Task<List<Guia>> ObtenerGuiasConDisponibilidadAsync();
        Task<Guia?> ObtenerGuiaConDisponibilidadAsync(string guiaId);

        Task<Guia?> ObtenerGuiaPersonalId(string userId);




    }
}
