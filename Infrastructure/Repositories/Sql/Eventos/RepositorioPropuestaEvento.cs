using Application.Eventos.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Eventos.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.Eventos
{
    internal sealed class RepositorioPropuestaEvento(MuseoDbContext context) : BaseRepository<PropuestaEvento>(context), IRepositorioPropuestaEvento
    {
        public async Task<List<PropuestaEvento>> FindAllAsync(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            if (fechaDesde == fechaHasta)
            {
                fechaHasta = fechaHasta.Date.AddDays(1).AddSeconds(-1);
            }


            var actividadesEnRango = await Repository
                .Include(p => p.Salas)
                .Include(p => p.Recursos)
                    .ThenInclude(r => r.Recurso)
                .Where(p =>
                    // Caso A: Actividades fijas individuales que caen justo en esta ventana
                    (!string.IsNullOrEmpty(p.RRule) && p.Fecha < fechaHasta && p.FechaHasta > fechaDesde) ||

                    (string.IsNullOrEmpty(p.RRule) && p.Fecha <= fechaHasta))
                .ToListAsync();



            return actividadesEnRango;
        }
    }
}
