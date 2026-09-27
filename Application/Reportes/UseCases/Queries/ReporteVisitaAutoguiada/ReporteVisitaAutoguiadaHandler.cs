using Application.Availability.Producers;
using Application.Reportes.ApplicationServices;
using Application.Reportes.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;

namespace Application.Reportes.UseCases.Queries.ReporteVisitanteSala
{
    internal sealed class ReporteVisitaAutoguiadaHandler(
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaAutoguiada,
        SelfGuidedAvailabilityProducerService selfGuidedAvailabilityProducer,
        IServicioReporteVisitasAutoguiadas servicioReporteVisitasAutoguiadas
        ) : IRequestQueryHandler<ReporteVisitaAutoguiadaQuery, ReporteVisitaAutoguiadaDto>
    {
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaAutoguiada = repositorioVisitaAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaAutoguiada));
        private readonly SelfGuidedAvailabilityProducerService _selfGuidedAvailabilityProducer = selfGuidedAvailabilityProducer ?? throw new ArgumentNullException(nameof(selfGuidedAvailabilityProducer));
        private readonly IServicioReporteVisitasAutoguiadas _servicioReporteVisitasAutoguiadas = servicioReporteVisitasAutoguiadas ?? throw new ArgumentNullException(nameof(servicioReporteVisitasAutoguiadas));

        public async Task<ReporteVisitaAutoguiadaDto> Handle(ReporteVisitaAutoguiadaQuery request, CancellationToken cancellationToken)
        {
            var visitasAutoguiadas= await _repositorioVisitaAutoguiada.GetAllGroupVisitAuAsync(request.Desde, request.Hasta);
            var SlotsDisponibles = await _selfGuidedAvailabilityProducer.GetHourlyBlocksAsync(
                request.Desde,
                request.Hasta
                );
             int totalCapacidadDisponible = SlotsDisponibles.Sum(s => s.CapacidadMaximaPorGrupo);
            var reporte = _servicioReporteVisitasAutoguiadas.GenerarReporteVisitasAutoguiadas(
               visitasAutoguiadas,
               totalCapacidadDisponible
               );
            return reporte;
        }
    }
}

