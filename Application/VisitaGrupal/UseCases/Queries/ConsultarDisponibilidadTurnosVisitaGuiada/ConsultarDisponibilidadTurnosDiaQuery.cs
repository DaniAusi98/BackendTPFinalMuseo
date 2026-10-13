using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaGuiada
{
    // Pasamos TurnoDisponibleDto. QueryResult se encarga de transformarlo en la lista (Items)
    public class ConsultarDisponibilidadTurnosDiaQuery(DateTime fechaDesde, DateTime fechaHasta, string? visitaId = null, List<string>? tematicaIds = null) : QueryRequest<QueryResult<TurnoDisponibleDto>>
    {
        public DateTime FechaDesde { get; } = fechaDesde;
        public DateTime FechaHasta { get; } = fechaHasta;
        public string? VisitaId { get; } = visitaId;
        public List<string>? TematicasIds { get; } = tematicaIds;

    }
}
