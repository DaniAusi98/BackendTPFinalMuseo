using Application.Availability.Models;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Microsoft.Extensions.Logging;

namespace Application.Availability.Rules
{
    public class NoConcurrentGuidedWithAutoguidedRule
        : IAvailabilityRule
    {
        private readonly ILogger<NoConcurrentGuidedWithAutoguidedRule>
            _logger;

        public NoConcurrentGuidedWithAutoguidedRule(
            ILogger<NoConcurrentGuidedWithAutoguidedRule> logger)
        {
            _logger = logger;
        }

        public Task<AvailabilityResult> CheckAsync(
            AvailabilityContext ctx,
            CandidateEntry entry)
        {
            var candidate = entry.Candidate;

            // Esta regla sólo aplica a visitas grupales.
            if (candidate is not VisitaGrupalGuiada &&
                candidate is not VisitaGrupalAutoguiada)
            {
                return Task.FromResult(
                    AvailabilityResult.Ok());
            }

            var candidateType =
                candidate is VisitaGrupalGuiada
                    ? "Guiada"
                    : "Autoguiada";

            _logger.LogDebug(
                "Checking concurrent visits for {CandidateType} candidate {CandidateId}",
                candidateType,
                candidate.Id);

            // Cada candidato puede tener uno o varios TimeSlots.
            foreach (var candidateSlot in entry.TimeSlots)
            {
                var conflict = ctx.ExistingActivities.Any(existing =>
                {
                    var existingActivity = existing.Actividad;

                    // Guiada contra autoguiada
                    if (candidate is VisitaGrupalGuiada &&
                        existingActivity is VisitaGrupalAutoguiada)
                    {
                        return existing.TimeSlots.Any(existingSlot =>
                            candidateSlot.SeSolapaCon(existingSlot));
                    }

                    // Autoguiada contra guiada
                    if (candidate is VisitaGrupalAutoguiada &&
                        existingActivity is VisitaGrupalGuiada)
                    {
                        return existing.TimeSlots.Any(existingSlot =>
                            candidateSlot.SeSolapaCon(existingSlot));
                    }

                    return false;
                });

                if (conflict)
                {
                    _logger.LogDebug(
                        "Concurrent visit conflict detected for {CandidateType} candidate {CandidateId}",
                        candidateType,
                        candidate.Id);

                    return Task.FromResult(
                        AvailabilityResult.Fail(
                            "No se permiten visitas guiadas y autoguiadas en el mismo horario."));
                }
            }

            _logger.LogDebug(
                "No concurrent visit conflicts for {CandidateType} candidate {CandidateId}",
                candidateType,
                candidate.Id);

            return Task.FromResult(
                AvailabilityResult.Ok());
        }
    }
}