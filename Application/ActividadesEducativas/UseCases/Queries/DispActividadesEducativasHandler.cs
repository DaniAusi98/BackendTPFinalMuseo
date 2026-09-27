using Application.ActividadesEducativas.DataTransferObjets;
using Application.Availability.Producers;
using Application.Eventos.UseCases.Queries;
using Core.Application;
using Microsoft.Extensions.Logging;


namespace Application.ActividadesEducativas.UseCases.Queries
{
    internal sealed class DispActividadesEducativasHandler(
      DispActEducativaProducerService producerService,
      ILogger<DispActividadesEducativasHandler> logger) : IRequestQueryHandler<DispActividadesEducativasQuery, DispActividadEducativaDto>
    {
        private readonly DispActEducativaProducerService _producerService = producerService
                ?? throw new ArgumentNullException(nameof(producerService));
        private readonly ILogger<DispActividadesEducativasHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<DispActividadEducativaDto> Handle(
           DispActividadesEducativasQuery request,
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
                var resultado = await _producerService.GetActEducativaAvailabilityAsync(
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
