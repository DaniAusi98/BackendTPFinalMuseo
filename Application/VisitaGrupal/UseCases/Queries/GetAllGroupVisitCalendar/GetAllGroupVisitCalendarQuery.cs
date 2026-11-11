using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetAllGroupVisitCalendar
{
    public class GetAllGroupVisitCalendarQuery(DateTime fechaConsultaDesde, DateTime fechaConsultaHasta) : IRequestQuery<VisitasGrupalesDto>
    {
        public DateTime FechaConsultaDesde { get; set; } = fechaConsultaDesde;
        public DateTime FechaConsultaHasta { get; set; } = fechaConsultaHasta;
    }
}
