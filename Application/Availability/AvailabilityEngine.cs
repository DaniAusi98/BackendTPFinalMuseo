using Application.Availability.Models;
using Domain.Common.ValueObjets;
using Microsoft.Extensions.Logging;

namespace Application.Availability
{
    public class AvailabilityEngine
    {
        private readonly IRuleFactory _ruleFactory;
        private readonly ILogger<AvailabilityEngine> _logger;

        public AvailabilityEngine(
            IRuleFactory ruleFactory,
            ILogger<AvailabilityEngine> logger)
        {
            _ruleFactory = ruleFactory;
            _logger = logger;
        }

        public async Task<AvailabilityResult> CheckAsync(
            AvailabilityContext ctx,
            CandidateEntry entry)
        {
            var candidate = entry.Candidate;

            var rules = _ruleFactory
                .GetRulesFor(candidate)
                .ToList();

            _logger.LogDebug(
                "Checking availability for {TipoActividad} (Id: {CandidateId}) with {RuleCount} rules",
                candidate.TipoActividad,
                candidate.Id,
                rules.Count);

            foreach (var rule in rules)
            {
                var ruleName = rule.GetType().Name;

                _logger.LogDebug(
                    "Executing rule: {RuleName}",
                    ruleName);

                var result = await rule.CheckAsync(
                    ctx,
                    entry);

                if (!result.IsOk)
                {
                    _logger.LogWarning(
                        "Rule {RuleName} failed for {TipoActividad} (Id: {CandidateId}): {Message}",
                        ruleName,
                        candidate.TipoActividad,
                        candidate.Id,
                        result.Message);

                    return result;
                }

                _logger.LogDebug(
                    "Rule {RuleName} passed",
                    ruleName);
            }

            _logger.LogDebug(
                "All rules passed for {TipoActividad} (Id: {CandidateId})",
                candidate.TipoActividad,
                candidate.Id);

            return AvailabilityResult.Ok();
        }

        public async Task<IDictionary<Guid, AvailabilityResult>>
            CheckManyAsync(
                AvailabilityContext baseCtx,
                IEnumerable<CandidateEntry> entries)
        {
            var results =
                new Dictionary<Guid, AvailabilityResult>();

            if (entries == null)
                return results;

            foreach (var entry in entries)
            {
                if (entry.TimeSlots == null ||
                    entry.TimeSlots.Count == 0)
                {
                    results[entry.Id] =
                        AvailabilityResult.Fail(
                            "El candidato no tiene horarios para verificar.");

                    continue;
                }

                var candidateStart =
                    entry.TimeSlots.Min(slot => slot.Inicio);

                var candidateEnd =
                    entry.TimeSlots.Max(slot => slot.Fin);

                var perCtx = new AvailabilityContext
                {
                    Start = candidateStart,
                    End = candidateEnd,
                    Metadata = baseCtx.Metadata,

                    ExistingActivities =
                        baseCtx.ExistingActivities
                            .Where(activity =>
                                activity.TimeSlots.Any(existingSlot =>
                                    entry.TimeSlots.Any(candidateSlot =>
                                        candidateSlot.SeSolapaCon(
                                            existingSlot))))
                            .ToList()
                };

                var result = await CheckAsync(
                    perCtx,
                    entry);

                results[entry.Id] = result;
            }

            return results;
        }
    }
}