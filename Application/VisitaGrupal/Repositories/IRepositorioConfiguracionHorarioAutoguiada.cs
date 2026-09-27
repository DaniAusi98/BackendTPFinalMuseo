using Core.Application.Repositories;
using Domain.VisitasGrupales.Entities;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioConfiguracionHorarioAutoguiada:IRepository<ConfiguracionHorarioAutoguiada>
    {
        Task<ConfiguracionHorarioAutoguiada?> ObtenerConfiguracionActivaAsync();

    }
}
