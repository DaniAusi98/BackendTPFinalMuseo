using System.Collections.Generic;

namespace Application.Availability
{
    // A provider is responsible for deciding if it can handle a candidate and returning the rules
    // that should be applied for that candidate. Providers receive their own dependencies via DI
    // and create rule instances that use those dependencies.
    public interface IRuleProvider
    {
        bool CanHandle(Domain.ActividadMuseo.Entities.ActividadMuseo candidate);
        IEnumerable<IAvailabilityRule> CreateRules(Domain.ActividadMuseo.Entities.ActividadMuseo candidate);
    }
}
