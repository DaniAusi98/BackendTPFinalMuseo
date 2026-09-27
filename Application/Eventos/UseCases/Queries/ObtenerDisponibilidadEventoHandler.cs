using Application.Availability.Producers;
using Application.Eventos.DataTransferObjets;
using Core.Application;
using Microsoft.Extensions.Logging;

namespace Application.Eventos.UseCases.Queries
{
    /// <summary>
    /// Handler que procesa la consulta de disponibilidad para eventos
    /// </summary>
    public class ObtenerDisponibilidadEventoHandler : 
        IRequestQueryHandler<ObtenerDisponibilidadEventoQuery, EventoDisponibilidadDto>
    {
        private readonly EventAvailabilityProducerService _producerService;
        private readonly ILogger<ObtenerDisponibilidadEventoHandler> _logger;

        public ObtenerDisponibilidadEventoHandler(
            EventAvailabilityProducerService producerService,
            ILogger<ObtenerDisponibilidadEventoHandler> logger)
        {
            _producerService = producerService 
                ?? throw new ArgumentNullException(nameof(producerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<EventoDisponibilidadDto> Handle(
            ObtenerDisponibilidadEventoQuery request, 
            CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.SalasIds == null || request.SalasIds.Count == 0)
                throw new ArgumentException("Debe especificar al menos una sala", nameof(request.SalasIds));

            _logger.LogInformation(
                "[ObtenerDisponibilidadEventoHandler] Procesando consulta de disponibilidad " +
                "para período {Desde} - {Hasta}, salas: {SalasCount}",
                request.Desde.Date,
                request.Hasta.Date,
                request.SalasIds.Count);

            try
            {
                var resultado = await _producerService.GetEventAvailabilityAsync(
                    request.Desde,
                    request.Hasta,
                    request.SalasIds);

                _logger.LogInformation(
                    "[ObtenerDisponibilidadEventoHandler] Disponibilidad calculada exitosamente. " +
                    "Total horas: {Total}, Disponibles: {Disponibles}",
                    resultado.HorariosDisponibles.Count,
                    resultado.HorariosDisponibles.Count(h => h.Disponible));

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "[ObtenerDisponibilidadEventoHandler] Error al calcular disponibilidad");

                throw;
            }
        }
    }
}
