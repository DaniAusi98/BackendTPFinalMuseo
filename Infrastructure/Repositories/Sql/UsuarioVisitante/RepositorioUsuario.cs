/*using Domain.CommonDomain.ValueObjets;

using Core.Infraestructure.Repositories.Sql;
using Application.Usuario.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Usuarios.Entities.
    +UsuarioVisitante;


namespace Infrastructure.Repositories.Sql.UsuarioVisitante
{
    internal class RepositorioUsuario(MuseoDbContext context) : BaseRepository<Visitante>(context), IRepositorioUsuarioVisitante
    {

        public async Task<bool> ExistsByEmailAsync(Email email)
        {
            return await context.UsuarioVisitante
           .AnyAsync(u => u.EmailVisitante.Valor == email.Valor);
        }

        public async Task<Visitante?> FindByEmailAsync(string email)
        {
            return await context.UsuarioVisitante
                .FirstOrDefaultAsync(u => u.EmailVisitante.Valor == email);
        }

        public async Task<Visitante> GetByIdAsync(object id)
        {
            return await FindOneAsync(id);
        }

        public async Task<Visitante> FindByTokenAsync(string token)
        {
            return await context.UsuarioVisitante
                .FirstOrDefaultAsync(u => u.TokenConfirmacion.Token == token);
        }
    }
}
*/
