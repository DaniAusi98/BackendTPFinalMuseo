using Application.ActividadMuseo.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.ActividadMuseo.Entities;

namespace Infrastructure.Repositories.Sql.ActividadMuseo
{
    internal sealed class RepositorioActividadReprogramada(MuseoDbContext context) : BaseRepository<ActividadReprogramada>(context), IRepositorioActividadReprogramada
    {
    }
}
