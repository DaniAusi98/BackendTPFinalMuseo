using Application.PersonalMuseo.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.PersonalMuseo.Entities.UsuarioInterno;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.PersonalMuseo
{
    internal sealed class RepositorioPersonal(MuseoDbContext context) : BaseRepository<Personal>(context), IRepositorioPersonal

    {
        public async Task<Personal?> GetPersonalMuseo(string userId)
        {
            return await Repository
              .Include(p => p.Asignaciones)
                .ThenInclude(a => a.AreaPuesto)
                   .ThenInclude(ap => ap.Area)
              .Include(p => p.Asignaciones)
                .ThenInclude(a => a.AreaPuesto)
                    .ThenInclude(ap => ap.Puesto)
             .FirstOrDefaultAsync(p => p.IdentityUserId == userId);

        }
    }
}
