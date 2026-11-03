using Application.Eventos.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Eventos.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.Eventos
{
    internal sealed class RepositorioPropuestaMuestra(MuseoDbContext context) : BaseRepository<PropuestaMuestra>(context), IRepositorioPropuestaMuestra
    {
        public async Task<List<PropuestaMuestra>> FindIncludeAllAsync()
        {
            return await Repository
                .Include(p => p.MuestraTemporal)
                   .ThenInclude(e => e.Salas)
                .ToListAsync();
        }

        public async Task<PropuestaMuestra?> FindIncludeByIdAsync(string id)
        {
            return await Repository
                .Include(p => p.MuestraTemporal)
                    .ThenInclude(e => e.Salas)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<PropuestaMuestra?> FindByEventoIdAsync(string muestraId)
        {
            return await Repository
                .FirstOrDefaultAsync(p => p.MuestraTemporalId == muestraId);
        }
    }
}
