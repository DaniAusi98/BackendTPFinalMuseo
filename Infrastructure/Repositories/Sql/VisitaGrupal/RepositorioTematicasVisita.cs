using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.VisitaGrupal.Repositories;

using Core.Infraestructure.Repositories.Sql;

using Domain.VisitasGrupales.Entities;

using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioTematicasVisita(MuseoDbContext context) : BaseRepository<TematicaVisita>(context), IRepositorioTematicas
    {
        public async Task<List<TematicaVisita>> ObtenerDisponiblesAsync()
        {
            return await Repository
                .Include(t => t.Salas)
                .Where(t => t.Disponible)
                .ToListAsync();
        }

        public async Task<List<TematicaVisita>> GetByIdsAsync(List<string> tematicasIds)
        {
            return await Repository
                .Include(t => t.Salas)
                .Where(t => tematicasIds.Contains(t.Id))
                .ToListAsync();
        }

        
    }
}
