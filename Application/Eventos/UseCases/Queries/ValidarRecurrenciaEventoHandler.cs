using Application.Eventos.ApplicationServices;
using Application.Eventos.DataTransferObjets;
using Core.Application;

namespace Application.Eventos.UseCases.Queries.ValidarRecurrenciaEvento
{
    internal sealed class ValidarRecurrenciaEventoHandler(
        IEventRecurrenceAvailabilityService service)
        : IRequestQueryHandler<
            ValidarRecurrenciaEventoQuery,
            ValidarRecurrenciaEventoDto>
    {
        private readonly IEventRecurrenceAvailabilityService _service =
            service ?? throw new ArgumentNullException(nameof(service));

        public async Task<ValidarRecurrenciaEventoDto> Handle(
            ValidarRecurrenciaEventoQuery request,
            CancellationToken cancellationToken)
        {
            return await _service.ValidarAsync(
                request.Inicio,
                request.Fin,
                request.SalasIds,
                request.RRule);
        }
    }
}