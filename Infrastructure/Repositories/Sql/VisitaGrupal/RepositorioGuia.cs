using Application.VisitaGrupal.Repositories;

using Core.Infraestructure.Repositories.Sql;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioGuia(MuseoDbContext context) : BaseRepository<Guia>(context), IRepositorioGuia
    {
        public async Task<List<Guia>> ObtenerDisponibilidadGuias(
            DateTime desde,
            DateTime hasta)
        {
            return await Repository
                .Include(g => g.HorariosGuia)
                .Include(g => g.AusenciasProgramadas
                    .Where(a => a.FechaDesde <= hasta &&
                                a.FechaHasta >= desde))
                .ToListAsync();
        }
        public async Task<List<Guia>> ObtenerGuiasConDisponibilidadAsync()
        {
            return await Repository
                .Include(g => g.HorariosGuia)
                .Include(g => g.AusenciasProgramadas)
                .ToListAsync();
        }
        public async Task<List<Guia>> ObtenerGuiasActivas()
        {

            return await Repository
                .Include(g => g.HorariosGuia)
                .Include(g => g.AusenciasProgramadas)
                .Where(a => a.Activo)
                 .ToListAsync();

        }
        public async Task<Guia?> ObtenerGuiaConDisponibilidadAsync(string guiaId)
        {
            return await Repository
                .Include(g => g.HorariosGuia)
                .Include(g => g.AusenciasProgramadas)
                .FirstOrDefaultAsync(g => g.Id == guiaId);
        }

        public async Task<Guia?> ObtenerGuiaPersonalId(string userId)
        {
            return await Repository
                .Include(g => g.HorariosGuia)
                .Include(g => g.AusenciasProgramadas)
                .FirstOrDefaultAsync(g => g.UserId == userId);
        }
    }
}
