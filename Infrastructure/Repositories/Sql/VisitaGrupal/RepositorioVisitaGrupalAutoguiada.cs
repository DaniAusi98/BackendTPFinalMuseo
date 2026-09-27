using Application.VisitaGrupal.Repositories;

using Core.Infraestructure.Repositories.Sql;

using Domain.VisitasGrupales.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioVisitaGrupalAutoguiada(MuseoDbContext context)
        : BaseRepository<VisitaGrupalAutoguiada>(context),
          IRepositorioVisitaGrupalAutoguiada
    {
        public async Task<List<VisitaGrupalAutoguiada>> GetAllGroupVisitAuAsync(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {

            if (fechaDesde == fechaHasta)
            {
                fechaHasta = fechaHasta.Date.AddDays(1).AddSeconds(-1); // Ajusta hasta el final del d�a
            }
            try
            {
                return await Repository
                    .Include(v => v.Salas)
                    .Where(v =>
                        v.Horario.Inicio < fechaHasta &&
                        v.Horario.Fin > fechaDesde)
                    .OrderBy(v => v.Horario.Inicio)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task<List<VisitaGrupalAutoguiada>> GetSelfGuidedToursByMonth(
        DateOnly monthDate)
        {
            return await Repository
                .Include(v => v.Horario)
                .Where(v => v.Horario.Inicio.Year == monthDate.Year &&
                            v.Horario.Fin.Month == monthDate.Month)
                .ToListAsync();
        }

        public async Task<List<VisitaGrupalAutoguiada>> ObtenerPorUsuarioIdAsync(
            string usuarioId,
            DateTime fechaActual)
        {
            return await Repository
                .Include(v => v.Horario)
                .Include(v => v.Salas)
                .Where(v =>
                    v.UsuarioVisitanteId == usuarioId &&
                    v.Horario.Inicio >= fechaActual)
                .ToListAsync();
        }
    }
}
