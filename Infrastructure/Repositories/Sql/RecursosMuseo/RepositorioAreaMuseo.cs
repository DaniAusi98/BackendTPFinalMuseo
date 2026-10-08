using Application.PersonalMuseo.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.PersonalMuseo.Entities.UsuarioInterno;

namespace Infrastructure.Repositories.Sql.RecursosMuseo
{
    internal sealed class RepositorioAreaMuseo(MuseoDbContext context) : BaseRepository<Area>(context), IRepositorioArea
    {
    }
}
