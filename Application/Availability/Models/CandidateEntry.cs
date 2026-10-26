using Domain.Common.ValueObjets;

namespace Application.Availability.Models
{
    public record CandidateEntry(
        Guid Id,
        Domain.ActividadMuseo.Entities.ActividadBase Candidate,
        IReadOnlyCollection<TimeSlot> TimeSlots,
        object? Original,
        string Source);
}