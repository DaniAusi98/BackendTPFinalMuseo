using Application.Common.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.Ubicacion
{
    /// <summary>
    /// Repositorio SQL para Localidad
    /// </summary>
    internal sealed class LocalidadRepository(MuseoDbContext context) : BaseRepository<LocalidadArg>(context), ILocalidadRepository
    {
        public async Task<List<LocalidadArg>> GetByDepartamentoIdAsync(string departamentoId,CancellationToken cancellationToken = default)
        {
            return await context.Localidades
                .Where(x => x.DepartamentoId == departamentoId)
                .OrderBy(x => x.Nombre)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<LocalidadArg>> GetByIdsAsync(IEnumerable<string> ids)
        {
            var listaIds = ids?.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct().ToList() ?? [];
            if (listaIds.Count == 0) return [];

            return await Repository.Where(l => listaIds.Contains(l.Id)).ToListAsync();
        }
    }
}
