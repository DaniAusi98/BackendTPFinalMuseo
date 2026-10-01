using Application.ActividadMuseo.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.DisponibilidadActividades
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
                fechaHasta = fechaHasta.Date.AddDays(1).AddSeconds(-1); // Ajusta hasta el final del dia
            }


            var actividadesEnRango = await Repository
                .Include(a => a.Salas)
                .Include(a => a.Recursos)
                    .ThenInclude(r => r.Recurso)
                .Include(a => a.Exceptions)
                .Where(a =>
                    // Caso A: Actividades fijas individuales que caen justo en esta ventana
                    (!string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio < fechaHasta && a.Horario.Fin > fechaDesde) ||

                    // Caso B: Actividades recurrentes que YA empezaron en el pasado o empiezan ahora.
                    // Si su primera cita hist�rica empez� despu�s de 'fechaHasta', es imposible que generen ocurrencias hoy.
                    (string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio <= fechaHasta))
                .ToListAsync();



            return actividadesEnRango;
        }
    }
}
