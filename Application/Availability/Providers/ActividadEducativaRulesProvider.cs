

using Application.Availability.Rules;

namespace Application.Availability.Providers
{
    internal class ActividadEducativaRulesProvider(NoConflictoSala noConflictoSala,
        NoActividadConEventoEnHall actSolapaEventoHall) : IRuleProvider
    {
        private readonly NoConflictoSala noConflictoSala = noConflictoSala;
        private readonly NoActividadConEventoEnHall noActividadConEventoEnHall = actSolapaEventoHall;

        public bool CanHandle(Domain.ActividadMuseo.Entities.ActividadBase candidate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAvailabilityRule> CreateRules(Domain.ActividadMuseo.Entities.ActividadBase candidate)
        {
            throw new NotImplementedException();
        }
    }
}
