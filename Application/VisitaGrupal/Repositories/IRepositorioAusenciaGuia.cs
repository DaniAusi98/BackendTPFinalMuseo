using Core.Application.Repositories;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioAusenciaGuia : IRepository<AusenciaGuia>
    {
        public Task<List<AusenciaGuia>> GetAllAsync(string guiaId, DateTime fechaDesde, DateTime fechaHasta);
    }
}
