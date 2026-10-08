using Core.Application.Repositories;

using Domain.RecursoMuseo.Entities.Guia;


namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioGuia : IRepository<Guia>
    {
        Task<List<Guia>> ObtenerGuiasConDisponibilidadAsync();
        Task<Guia?> ObtenerGuiaConDisponibilidadAsync(string guiaId);

        Task<Guia?> ObtenerGuiaPersonalId(string personalId);




    }
}
