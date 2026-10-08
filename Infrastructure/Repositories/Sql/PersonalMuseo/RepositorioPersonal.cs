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
             .FirstOrDefaultAsync(p => p.IdentityUserId == userId);

        }
    }
}
