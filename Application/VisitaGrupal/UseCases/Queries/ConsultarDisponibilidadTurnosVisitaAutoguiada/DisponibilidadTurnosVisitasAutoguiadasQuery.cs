using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;
using Domain.VisitasGrupales.Entities;


namespace Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaAutoguiada
{
    public class DisponibilidadTurnosVisitasAutoguiadasQuery(DateTime fechaDesde, DateTime fechaHasta) : QueryRequest<QueryResult<SlotDisponibleDto>>
    {
        public DateTime FechaDesde { get; } = fechaDesde;
        public DateTime FechaHasta { get; } = fechaHasta;


    }
}
