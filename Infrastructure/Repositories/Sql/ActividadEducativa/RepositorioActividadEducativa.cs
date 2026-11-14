using Application.ActividadesEducativas.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Microsoft.EntityFrameworkCore;
using static Domain.ActividadMuseo.Enums.Enums;


namespace Infrastructure.Repositories.Sql.ActividadEducativa
{
    internal sealed class RepositorioActividadEducativa(MuseoDbContext context) : BaseRepository<Domain.ActividadesAreaEducacion.Entities.ActividadEducativa>(context), IRepositorioActividadEducativa
    {
        public async Task<List<Domain.ActividadesAreaEducacion.Entities.ActividadEducativa>> GetAllActEducationAsync(
          DateTime fechaDesde,
            DateTime fechaHasta)
        {
            if (fechaDesde == fechaHasta)
            {
                fechaHasta = fechaHasta.Date.AddDays(1).AddSeconds(-1); // Ajusta hasta el final del dia
            }

            // ============================================================
            // CONSULTA OPTIMIZADA: Trae candidatos potenciales
            // ============================================================
            var actividadEducativas = await Repository
                .Include(a => a.Salas)
                .Include(a => a.Recursos)
                    .ThenInclude(r => r.Recurso)
                .Include(a => a.ProyectoVinculada)
                .ThenInclude(p => p.EquipoTrabajoResponsable)
                    .Include(a => a.Exceptions)

                    .Where(a =>
                    // Caso A: Actividades fijas individuales que caen justo en esta ventana
                    (!string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio < fechaHasta && a.Horario.Fin > fechaDesde) ||

                    // Caso B: Actividades recurrentes que YA empezaron en el pasado o empiezan ahora.
                    // Si su primera cita hist�rica empez� despu�s de 'fechaHasta', es imposible que generen ocurrencias hoy.
                    (string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio <= fechaHasta))
                .Where(a => a.Estado == EstadoActividad.Activa)
                .OrderByDescending(a => a.Horario.Inicio)
                .ToListAsync();


            return actividadEducativas;
        }
        public async Task<Domain.ActividadesAreaEducacion.Entities.ActividadEducativa?> FindOneIncludeAsync(string id)
        {
            return await Repository
                .Include(e => e.Salas)
                .Include(e => e.ProyectoVinculada)
                .Include(e => e.Recursos)
                    .ThenInclude(r => r.Recurso)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
