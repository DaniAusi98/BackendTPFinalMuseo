using Application.Eventos.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Eventos.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.Eventos
{
    internal sealed class RepositorioEvento(MuseoDbContext context) : BaseRepository<Evento>(context), IRepositorioEvento
    {
        public async Task<List<Evento>> FindAllAsync(
    DateTime fechaDesde,
    DateTime? fechaHasta)
        {
            if (fechaHasta == null)
            {
                fechaHasta = DateTime.Today.AddDays(1).AddSeconds(-1);
            }
            else if (fechaDesde.Date == fechaHasta.Value.Date)
            {
                fechaHasta = fechaHasta.Value.Date.AddDays(1).AddSeconds(-1);
            }

            var eventosEnRango = await Repository
                .Include(a => a.Salas)
                .Include(a => a.Recursos)
                    .ThenInclude(r => r.Recurso)
                .Include(a => a.Exceptions)
                .Where(a =>
                    // Actividades recurrentes
                    (!string.IsNullOrEmpty(a.RRule) &&
                     a.Horario.Inicio < fechaHasta.Value &&
                     a.Horario.Fin > fechaDesde) ||

                    // Actividades no recurrentes
                    (string.IsNullOrEmpty(a.RRule) &&
                     a.Horario.Inicio <= fechaHasta.Value))
                .OrderByDescending(a => a.Horario.Inicio)
                .ToListAsync();

            return eventosEnRango;
        }


        public async Task<Evento?> FindOneIncludeAsync(string id)
        {
            return await Repository
                .Include(e => e.Salas)
                .Include(e => e.Recursos)
                    .ThenInclude(r => r.Recurso)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
