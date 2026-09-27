using Application.MuseumResources.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.RecursoMuseo.Entities;
using Microsoft.EntityFrameworkCore;
using static Domain.RecursoMuseo.Enums.Enums;

namespace Infrastructure.Repositories.Sql.RecursosMuseo
{
    internal sealed class RepositorioRecursoMuseo(MuseoDbContext context) : BaseRepository<Recurso>(context), IRepositorioRecurso
    {
        public async Task<List<Recurso>> FindActivosAsync()
        {
            return await Repository
                .Where(r => r.Estado == EstadoRecurso.Disponible)
                .ToListAsync();
        }
    }
}

