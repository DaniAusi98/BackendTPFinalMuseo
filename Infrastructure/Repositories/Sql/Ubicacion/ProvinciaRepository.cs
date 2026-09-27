using Application.Common.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.Ubicacion
{
    /// <summary>
    /// Repositorio SQL para Provincia
    /// </summary>
    internal sealed class ProvinciaRepository(MuseoDbContext context) : BaseRepository<Provincia>(context), IProvinciaRepository
    {
        public async Task<List<Provincia>> GetByIdsAsync(IEnumerable<string> ids)
        {
            var listaIds = ids?.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct().ToList() ?? [];
            if (listaIds.Count == 0) return [];

            return await Repository.Where(p => listaIds.Contains(p.Id)).ToListAsync();
        }
    }
}
