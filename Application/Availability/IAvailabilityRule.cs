using Application.Availability.Models;

namespace Application.Availability
{
    public interface IAvailabilityRule
    {
        Task<AvailabilityResult> CheckAsync(
            AvailabilityContext ctx,
            CandidateEntry entry);
    }
}