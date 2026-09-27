using Application.Eventos.DataTransferObjets;

namespace Application.Eventos.ApplicationServices
{
    public interface IEventRecurrenceAvailabilityService
    {
        Task<ValidarRecurrenciaEventoDto> ValidarAsync(
        DateTime inicio,
        DateTime fin,
        List<string> salasIds,
        string rrule);
    }
}
