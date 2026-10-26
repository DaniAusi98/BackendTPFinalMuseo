using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;

namespace Application.Availability.Models
{
    public class ActivityAvailabilityEntry(
        ActividadBase actividad,
        IReadOnlyCollection<TimeSlot> timeSlots)
    {
        public ActividadBase Actividad { get; } = actividad;

        public IReadOnlyCollection<TimeSlot> TimeSlots { get; } = timeSlots;
    }
}

