using Application.MuseumResources.Repositories;

using Core.Infraestructure.Repositories.Sql;

using Domain.RecursoMuseo.Entities;
using Microsoft.EntityFrameworkCore;
using static Domain.RecursoMuseo.Enums.Enums;

namespace Infrastructure.Repositories.Sql.RecursosMuseo
{
    internal sealed class RepositorioSalaMuseo(MuseoDbContext context) : BaseRepository<Sala>(context), IRepositorioSala
    {
        public async Task<List<Sala>> ObtenerSalasporIdsAsync(IEnumerable<string> ids)
        {
            return await context.Set<Sala>()
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }
        public async Task<List<Sala>> ObtenerSalasDisponiblesAsync()
        {
            return await context.Set<Sala>()
                .Where(s => s.EstadoSala == EstadoSala.Activa)
                .ToListAsync();
        }
    }
}

