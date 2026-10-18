using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Guias.Queries.DisponibilidadGuias
{
    public class DisponibilidadGuiasCalendarQuery(DateTime fechaDesde, DateTime fechaHasta) : IRequestQuery<DisponibilidadGuiasDto>
    {
        public DateTime FechaDesde { get; } = fechaDesde;
        public DateTime FechaHasta { get; } = fechaHasta;

    }
}
