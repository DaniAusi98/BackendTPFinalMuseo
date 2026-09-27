using Application.ActividadesEducativas.Repositories;
using Application.ActividadMuseo.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.ActividadesAreaEducacion.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories.Sql.ActividadEducativa
{
    internal sealed class RepositorioProyectoEducativo(MuseoDbContext context) : BaseRepository<ProyectosAreaEducacion>(context), IRepositorioProyectoAreaEducacion
    {
        public async Task<List<ProyectosAreaEducacion>> FindAllAsyncIncludes()
        {
            return await context.ProyectosAreaEducacion
                .Include(x => x.EquipoTrabajoResponsable)
                .ToListAsync();
        }
    }
}
