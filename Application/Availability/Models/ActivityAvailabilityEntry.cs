using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common.ValueObjets;

namespace Application.Availability.Models
    {
        public class ActivityAvailabilityEntry
        {
            public Domain.ActividadMuseo.Entities.ActividadMuseo Actividad { get; }

            public IReadOnlyCollection<TimeSlot> TimeSlots { get; }

            public ActivityAvailabilityEntry(
                Domain.ActividadMuseo.Entities.ActividadMuseo actividad,
                IReadOnlyCollection<TimeSlot> timeSlots)
            {
                Actividad = actividad;
                TimeSlots = timeSlots;
            }
        }
    }

