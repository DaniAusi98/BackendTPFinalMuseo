using Application.Eventos.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Eventos.Entities;
using Microsoft.EntityFrameworkCore;
using static Domain.ActividadMuseo.Enums.Enums;

namespace Infrastructure.Repositories.Sql.Eventos
{
    internal sealed class RepositorioPropuestaEvento(MuseoDbContext context) : BaseRepository<PropuestaEvento>(context), IRepositorioPropuestaEvento
    {
        public async Task<List<PropuestaEvento>> FindIncludeAllAsync()
        {
            return await Repository
                .Where(p => p.Evento.Estado == EstadoActividad.Activa)
                .Include(p => p.Evento)
                    .ThenInclude(e => e.Salas)
                .ToListAsync();
        }

        public async Task<PropuestaEvento?> FindIncludeByIdAsync(string id)
        {
            return await Repository
                .Include(p => p.Evento)
                    .ThenInclude(e => e.Salas)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<PropuestaEvento?> FindByEventoIdAsync(string eventoId)
        {
            return await Repository
                .FirstOrDefaultAsync(p => p.EventoId == eventoId);
        }
    }
}
