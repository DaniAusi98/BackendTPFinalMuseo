using Application.Common.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Common.Entities;

namespace Infrastructure.Repositories.Sql.RecursosMuseo
{
    internal sealed class RepositorioBloqueoSala(MuseoDbContext context) : BaseRepository<BloqueoSala>(context), IRepositorioBloqueoSala
    {
    }
}
