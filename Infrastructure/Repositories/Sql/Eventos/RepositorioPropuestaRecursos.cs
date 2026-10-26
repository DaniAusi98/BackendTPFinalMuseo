using Application.Eventos.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Eventos.Entities;

namespace Infrastructure.Repositories.Sql.Eventos
{
    internal sealed class RepositorioPropuestaRecursos(MuseoDbContext context) : BaseRepository<PropuestasRecursos>(context), IRepositorioPropuestaRecursos
    {
    }
}
