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
           DateTime fechaHasta)
        {
            if (fechaDesde == fechaHasta)
            {
                fechaHasta = fechaHasta.Date.AddDays(1).AddSeconds(-1);
            }


            var eventosEnRango = await Repository
                .Include(a => a.Salas)
                .Include(a => a.Recursos)
                    .ThenInclude(r => r.Recurso)
                .Include(a => a.Exceptions)
                .Where(a =>
                    // Caso A: Actividades fijas individuales que caen justo en esta ventana
                    (!string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio < fechaHasta && a.Horario.Fin > fechaDesde) ||

                    // Caso B: Actividades recurrentes que YA empezaron en el pasado o empiezan ahora.
                    (string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio <= fechaHasta))

                .OrderByDescending(a => a.Horario.Inicio)
                .ToListAsync();


            return eventosEnRango;
        }
    }
}
