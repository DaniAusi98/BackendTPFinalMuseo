using Application.ApplicationMuseo.DataTransferObjects;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Guias.Queries
{
    public class ConsultarDisponibilidadGuiasQuery(DateTime fechaDesde, DateTime fechaHasta) : IRequestQuery<DummyEntityDto>
    {
        public DateTime FechaDesde { get; } = fechaDesde;
        public DateTime FechaHasta { get; } = fechaHasta;
    }
}
