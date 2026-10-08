using Application.PersonalMuseo.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.PersonalMuseo.Entities.UsuarioInterno;

namespace Infrastructure.Repositories.Sql.PersonalMuseo
{
    internal sealed class RepositorioPuesto(MuseoDbContext context) : BaseRepository<Puesto>(context), IRepositorioPuesto
    {
    }
}
