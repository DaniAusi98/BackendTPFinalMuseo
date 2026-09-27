using Application.Availability.Producers;
using Application.Reportes.ApplicationServices;
using Application.Reportes.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;

namespace Application.Reportes.UseCases.Queries.ReporteVisitaGuiada
{
    internal sealed class ReporteVisitaGuiadaHandler(
        IRepositorioVisitaGuiada repositorioVisitaGuiada,
        GuidedAvailabilityProducerService guidedAvailabilityProducer,
        IServicioReporteVisitasGuiadas servicioReporteVisitasGuiadas


        ) : IRequestQueryHandler<ReporteVisitaGuiadaQuery, ReporteVisitaGuiadaDto>
    {
        private readonly IRepositorioVisitaGuiada 
            _repositorioVisitaGuiada= 
            repositorioVisitaGuiada 
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        private readonly GuidedAvailabilityProducerService 
            _guidedAvailabilityProducer= 
            guidedAvailabilityProducer 
            ?? throw new ArgumentNullException(nameof(guidedAvailabilityProducer));
        private readonly IServicioReporteVisitasGuiadas _servicioReporteVisitasGuiadas = servicioReporteVisitasGuiadas ?? throw new ArgumentNullException(nameof(servicioReporteVisitasGuiadas));
        public async Task<ReporteVisitaGuiadaDto> Handle(ReporteVisitaGuiadaQuery request, CancellationToken cancellationToken)
        {
            var visitasguiadas =await _repositorioVisitaGuiada.GetAllGroupVisitAsync(request.Desde, request.Hasta);
            var TurnosDisponibles = await _guidedAvailabilityProducer.GetAvailableTurnsAsync(
                request.Desde,
                request.Hasta
                );
            int totalCapacidadDisponible = TurnosDisponibles.Sum(t => t.cuposDisponibles);
            var reporte = _servicioReporteVisitasGuiadas.GenerarReporteGeneralVisitasGuiadas(
                visitasguiadas,
                totalCapacidadDisponible
                );
            return reporte;
        }
    }
}
