using Application.PersonalMuseo.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.PersonalMuseo.Entities.UsuarioInterno;

namespace Infrastructure.Repositories.Sql.PersonalMuseo
{
    internal sealed class RepositorioAsignacionPuestoPersonal(MuseoDbContext context) : BaseRepository<AsignacionPersonal>(context), IRepositorioAsignacionPuestoPersonal
    {
    }
}
