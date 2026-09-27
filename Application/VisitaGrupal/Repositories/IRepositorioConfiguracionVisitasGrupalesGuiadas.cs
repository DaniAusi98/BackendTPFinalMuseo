using Core.Application.Repositories;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioConfiguracionVisitasGrupalesGuiadas : IRepository<ConfiguracionVisitasGrupalesGuiadas>
    {
        Task<ConfiguracionVisitasGrupalesGuiadas?> ObtenerConfiguracionActivaAsync();
    }
}
