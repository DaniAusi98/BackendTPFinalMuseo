using Application.Eventos.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Eventos.Entities;

namespace Infrastructure.Repositories.Sql.Eventos
{
    internal sealed class RepositorioMuestraTemporal(MuseoDbContext context) : BaseRepository<MuestraTemporal>(context), IRepositorioMuestraTemporal
    {
    }
}
