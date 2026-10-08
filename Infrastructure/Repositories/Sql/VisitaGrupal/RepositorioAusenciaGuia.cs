using Application.VisitaGrupal.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.RecursoMuseo.Entities.Guia;
using Microsoft.EntityFrameworkCore; // Asegúrate de tener esta importación

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioAusenciaGuia(MuseoDbContext context) : BaseRepository<AusenciaGuia>(context), IRepositorioAusenciaGuia
    {
        public async Task<List<AusenciaGuia>> GetAllAsync(string guiaId, DateTime fechaDesde, DateTime fechaHasta)
        {
            return await Repository // O la propiedad/campo que exponga tu BaseRepository (ej. Repository)
                .Where(x => x.GuiaId == guiaId &&
                            x.FechaDesde < fechaHasta &&
                            x.FechaHasta > fechaDesde)
                .ToListAsync();
        }
    }
}
