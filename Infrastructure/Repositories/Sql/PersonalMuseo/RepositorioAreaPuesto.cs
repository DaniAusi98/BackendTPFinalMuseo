using Application.PersonalMuseo.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.PersonalMuseo.Entities.UsuarioInterno;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.PersonalMuseo
{
    internal sealed class RepositorioAreaPuesto(MuseoDbContext context) : BaseRepository<AreaPuesto>(context), IRepositorioAreaPuesto
    {
        public async Task<List<Puesto>> ObtenerPuestosPorArea(string areaId)
        {
            var puestos = await Repository.Where(p => p.AreaId == areaId).Select(p => p.Puesto).ToListAsync();
            return puestos;
        }
    }
}
