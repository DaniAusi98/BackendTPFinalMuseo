using Application.VisitaGrupal.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.VisitasGrupales.Entities.GrupalAutoguiada;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioConfiguracionHorarioAutoguiada(MuseoDbContext context)
        : BaseRepository<ConfiguracionHorarioAutoguiada>(context), IRepositorioConfiguracionHorarioAutoguiada
    {


        async Task<ConfiguracionHorarioAutoguiada?> IRepositorioConfiguracionHorarioAutoguiada.ObtenerConfiguracionActivaAsync()
        {
            return await Repository.FirstOrDefaultAsync();
        }


    }
}


