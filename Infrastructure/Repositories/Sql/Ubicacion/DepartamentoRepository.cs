using Application.Common.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.Ubicacion
{
    /// <summary>
    /// Repositorio SQL para Departamento
    /// </summary>
    internal sealed class DepartamentoRepository(MuseoDbContext context) : BaseRepository<Departamento>(context), IDepartamentoRepository
    {
        public async Task<List<Departamento>> GetDepartamentoByProvinciaIdAsync(
            string provinciaId,
            CancellationToken cancellationToken = default)
        {
            return await context.Departamentos
                .Where(x => x.ProvinciaId == provinciaId)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<Departamento>> GetByIdsAsync(IEnumerable<string> ids)
        {
            var listaIds = ids?.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct().ToList() ?? [];
            if (listaIds.Count == 0) return [];

            return await Repository.Where(d => listaIds.Contains(d.Id)).ToListAsync();
        }
    }
}
