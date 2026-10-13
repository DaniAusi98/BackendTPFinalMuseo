using Application.PersonalMuseo.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.PersonalMuseo.Entities.UsuarioInterno;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.PersonalMuseo
{
    internal sealed class RepositorioAreaPuesto(MuseoDbContext context) : BaseRepository<AreaPuesto>(context), IRepositorioAreaPuesto
    {
        public async Task<List<AreaPuesto>> ObtenerPuestosPorArea(string areaId)
        {
            var puestosArea = await Repository
                .Include(a => a.Area)
                .Include(a => a.Puesto)
                .Where(p => p.AreaId == areaId).ToListAsync();
            return puestosArea;
        }
    }
}
