using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Guias.Queries.DisponibilidadGuia
{
    public class DisponGuiaUserIdQuery(DateTime fechaDesde, DateTime fechaHasta, string userId) : IRequestQuery<DisponibilidadGuiaDto>
    {
        public DateTime FechaDesde { get; } = fechaDesde;
        public DateTime FechaHasta { get; } = fechaHasta;
        public string UserId { get; } = userId;
    }
}
