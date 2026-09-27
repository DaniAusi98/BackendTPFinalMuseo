using Application.VisitaGrupal.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioConfiguracionVisitasGrupalesGuiadas(MuseoDbContext context) 
        : BaseRepository<ConfiguracionVisitasGrupalesGuiadas>(context), IRepositorioConfiguracionVisitasGrupalesGuiadas
    {
        public async Task<ConfiguracionVisitasGrupalesGuiadas?> ObtenerConfiguracionActivaAsync()
        {
            return await Repository.FirstOrDefaultAsync();
        }
    }
}
