using Application.VisitaGrupal.Repositories;

using Core.Infraestructure.Repositories.Sql;

using Domain.RecursoMuseo.Entities.Guia;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioGuia(MuseoDbContext context) : BaseRepository<Guia>(context), IRepositorioGuia
    {
        public async Task<List<Guia>> ObtenerGuiasConDisponibilidadAsync()
        {
            // HorariosGuia was converted to a value object; EF Include on it is invalid.
            // Include only the navigational collection of absences to avoid EF Core Include errors.
            return await Repository
                .Include(g=> g.HorariosGuia)
                .Include(g => g.AusenciasProgramadas)
                .ToListAsync();
        }
    }
}
