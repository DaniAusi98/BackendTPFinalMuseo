using Application.Eventos.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Eventos.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.Eventos
{
    internal sealed class RepositorioMuestraTemporal(MuseoDbContext context)
        : BaseRepository<MuestraTemporal>(context), IRepositorioMuestraTemporal
    {
        public async Task<List<MuestraTemporal>> FindAllIncludeAsync()
        {
            return await Repository
                .Include(a => a.Salas)
                .Include(a => a.UrlImagenesMuestra)
                .Include(a => a.Recursos)
                    .ThenInclude(r => r.Recurso)
                .OrderByDescending(a => a.Horario.Inicio)
                .ToListAsync();
        }

        public async Task<List<MuestraTemporal>> FindAllIncludeByPeriodAsync(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            return await Repository
                .Where(m =>
                    m.Horario.Inicio <= fechaHasta &&
                    m.Horario.Fin >= fechaDesde)
                .Include(a => a.Salas)
                .Include(a => a.Recursos)
                    .ThenInclude(r => r.Recurso)
                .OrderByDescending(a => a.Horario.Inicio)
                .ToListAsync();
        }

        public async Task<MuestraTemporal?> FindOneIncludeAsync(string id)
        {
            return await Repository
                .Include(e => e.Salas)
                .Include(e => e.Recursos)
                    .ThenInclude(r => r.Recurso)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}