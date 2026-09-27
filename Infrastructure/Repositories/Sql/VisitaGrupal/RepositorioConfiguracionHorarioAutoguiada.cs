using Application.VisitaGrupal.Repositories;
using Core.Application.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.VisitasGrupales.Entities;
using Infrastructure.Repositories.Sql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


