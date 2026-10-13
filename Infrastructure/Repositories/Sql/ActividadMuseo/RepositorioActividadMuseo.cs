using Application.ActividadMuseo.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Microsoft.EntityFrameworkCore;
using static Domain.ActividadMuseo.Enums.Enums;

namespace Infrastructure.Repositories.Sql.ActividadMuseo
{
    internal sealed class RepositorioActividadMuseo(MuseoDbContext context)
        : BaseRepository<Domain.ActividadMuseo.Entities.ActividadMuseo>(context), IRepositorioActividadMuseo
    {
        public async Task<List<Domain.ActividadMuseo.Entities.ActividadMuseo>> FindAllAsync(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            if (fechaDesde == fechaHasta)
            {
                fechaHasta = fechaHasta.Date.AddDays(1).AddSeconds(-1);
            }


            var actividadesEnRango = await Repository
                .Include(a => a.Salas)
                .Include(a => a.Recursos)
                    .ThenInclude(r => r.Recurso)
                .Include(a => a.Exceptions)
                .Where(a =>
                    // Caso A: Actividades fijas individuales que caen justo en esta ventana
                    (!string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio < fechaHasta && a.Horario.Fin > fechaDesde) ||

                    (string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio <= fechaHasta))
                .ToListAsync();



            return actividadesEnRango;
        }

        public async Task<List<Domain.ActividadMuseo.Entities.ActividadMuseo>> FindAllCalendarAsync(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            if (fechaDesde == fechaHasta)
            {
                fechaHasta = fechaHasta.Date.AddDays(1).AddSeconds(-1);
            }


            var actividadesEnRango = await Repository
                .Include(a => a.Salas)
                .Include(a => a.Recursos)
                    .ThenInclude(r => r.Recurso)
                .Include(a => a.Exceptions)
                .Where(a =>
                    // Caso A: Actividades fijas individuales que caen justo en esta ventana
                    (!string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio < fechaHasta && a.Horario.Fin > fechaDesde) ||

                    (string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio <= fechaHasta))
                .Where(a => a.Estado == EstadoActividad.Activa)
                .ToListAsync();



            return actividadesEnRango;
        }
    }
}
