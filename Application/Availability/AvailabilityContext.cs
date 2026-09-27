using Application.Availability.Models;

namespace Application.Availability
{
    public class AvailabilityContext
    {
        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        public IReadOnlyCollection<ActivityAvailabilityEntry>ExistingActivities{ get; init; }= new List<ActivityAvailabilityEntry>();

        public IReadOnlyDictionary<string, object> Metadata { get; init; } = new Dictionary<string, object>();
    }
}