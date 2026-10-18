using Core.Application.Repositories;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioAusenciaGuia : IRepository<AusenciaGuia>
    {
        public Task<List<AusenciaGuia>> GetAllAsyncByGuideId(string guiaId, DateTime fechaDesde, DateTime fechaHasta);
        public Task<List<AusenciaGuia>> GetAllAsync();
        Task<List<AusenciaGuia>> GetAllWithGuiaAsync();



    }
}
