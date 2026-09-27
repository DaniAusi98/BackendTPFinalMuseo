using Domain.Common.ValueObjets;

namespace Application.Availability.Models
{
    public record CandidateEntry(
        Guid Id,
        Domain.ActividadMuseo.Entities.ActividadMuseo Candidate,
        IReadOnlyCollection<TimeSlot> TimeSlots,
        object? Original,
        string Source);
}