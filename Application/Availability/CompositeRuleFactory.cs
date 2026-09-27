namespace Application.Availability
{
    public interface IRuleFactory
    {
        IEnumerable<IAvailabilityRule> GetRulesFor(Domain.ActividadMuseo.Entities.ActividadMuseo candidate);
    }

    public class CompositeRuleFactory : IRuleFactory
    {
        private readonly IEnumerable<IRuleProvider> _providers;

        public CompositeRuleFactory(IEnumerable<IRuleProvider> providers)
        {
            _providers = providers;
        }

        public IEnumerable<IAvailabilityRule> GetRulesFor(Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            return _providers
                .Where(p => p.CanHandle(candidate))
                .SelectMany(p => p.CreateRules(candidate))
                .ToList();
        }
    }
}
