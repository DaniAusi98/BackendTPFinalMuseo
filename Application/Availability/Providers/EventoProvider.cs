
using Application.Availability.Rules;
using Domain.Eventos.Entities;
namespace Application.Availability.Providers
{
    public class EventoProvider(
        NoEventoHallConActividad noEventoHallConActividad,
        NoConflictoSala noConflictoSala) : IRuleProvider
    {
        private readonly NoEventoHallConActividad noEventoHallConActividad = noEventoHallConActividad;
        private readonly NoConflictoSala noConflictoSala = noConflictoSala;

        public bool CanHandle(Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            return candidate is Evento;
                
        }

        public IEnumerable<IAvailabilityRule> CreateRules(Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            yield return noEventoHallConActividad;
            yield return noConflictoSala;
        }
    }
}
