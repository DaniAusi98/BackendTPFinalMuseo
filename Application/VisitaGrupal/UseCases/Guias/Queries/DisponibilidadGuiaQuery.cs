using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Guias.Queries
{
    public class DisponibilidadGuiaQuery(DateTime fechaDesde, DateTime fechaHasta, string guiaId) : IRequestQuery<DisponibilidadGuiaDto>
    {
        public DateTime FechaDesde { get; } = fechaDesde;
        public DateTime FechaHasta { get; } = fechaHasta;
        public string GuiaId { get; } = guiaId;
    }
}
