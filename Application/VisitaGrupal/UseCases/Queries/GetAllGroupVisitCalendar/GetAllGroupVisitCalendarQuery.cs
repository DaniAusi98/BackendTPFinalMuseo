using Application.VisitaGrupal.DataTransferObjets;

using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetAllGroupVisitCalendar
{
    public class GetAllGroupVisitCalendarQuery(DateTime fechaConsultaDesde, DateTime fechaConsultaHasta) : QueryRequest<QueryResult<VisitaGrupalCalendarDto>>
    {
        public DateTime FechaConsultaDesde { get; } = fechaConsultaDesde;
        public DateTime FechaConsultaHasta { get; } = fechaConsultaHasta;
    }
}
