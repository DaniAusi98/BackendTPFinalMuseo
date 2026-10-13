using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;


namespace Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaAutoguiada
{
    public class DisponibilidadTurnosVisitasAutoguiadasQuery(DateTime fechaDesde, DateTime fechaHasta, string? visitaId = null, List<string>? tematicaIds = null) : QueryRequest<QueryResult<SlotDisponibleDto>>
    {
        public DateTime FechaDesde { get; } = fechaDesde;
        public DateTime FechaHasta { get; } = fechaHasta;
        public string? VisitaId { get; } = visitaId;
        public List<string>? TematicasIds { get; } = tematicaIds;


    }
}
