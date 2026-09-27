using Application.MuseumResources.Repositories;
using Core.Application.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.RecursoMuseo.Entities;
using Microsoft.EntityFrameworkCore;
using static Domain.ActividadMuseo.Enums.Enums;

namespace Infrastructure.Repositories.Sql.RecursosMuseo
{
    internal sealed class RepositorioConfiguracionSalaActividad(MuseoDbContext context) : BaseRepository<ConfiguracionSalaActividad>(context), IRepositorioConfiguracionSalaActividad
    {

       
        public async Task<List<ConfiguracionSalaActividad>> ObtenerPorSalaAsync(string salaId)
        {
            return await context.ConfiguracionesSalaActividad
                .Where(c => c.SalaId == salaId)
                .ToListAsync();
        }

        public async Task<List<ConfiguracionSalaActividad>> ObtenerPorTipoActividadAsync(TipoActividad tipoActividad)
        {
            return await context.ConfiguracionesSalaActividad
                .Where(c => c.TipoActividad == tipoActividad && c.Habilitada)
                .ToListAsync();
        }

        public async Task<ConfiguracionSalaActividad?> ObtenerPorSalaYActividadAsync(
            string salaId,
            TipoActividad tipoActividad)
        {
            return await context.ConfiguracionesSalaActividad
                .FirstOrDefaultAsync(c => c.SalaId == salaId && c.TipoActividad == tipoActividad);
        }

        public async Task EliminarPorSalaAsync(string salaId)
        {
            var configuraciones = await context.ConfiguracionesSalaActividad
                .Where(c => c.SalaId == salaId)
                .ToListAsync();

            context.ConfiguracionesSalaActividad.RemoveRange(configuraciones);
            await context.SaveChangesAsync();
        }
    }
}
