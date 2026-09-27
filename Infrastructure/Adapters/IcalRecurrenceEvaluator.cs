using Application.Availability.ApplicationServices;
using Application.Eventos.DataTransferObjets;
using Domain.Common.ValueObjets;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

using IcalDuration = Ical.Net.DataTypes.Duration;

namespace Infrastructure.Adapters
{
    public class IcalRecurrenceEvaluator : IRecurrenceEvaluator
    {
        public List<TimeSlot> ExpandRule(
            string rrule,
            DateTime eventStart,
            int durationMinutes,
            DateTime windowStart,
            DateTime windowEnd)
        {
            var slots = new List<TimeSlot>();

            var pattern = new RecurrencePattern(rrule);

            var virtualEvent = new CalendarEvent
            {
                Start = new CalDateTime(eventStart),
                Duration = IcalDuration.FromMinutes(durationMinutes),
                RecurrenceRule = pattern
            };

            var iCalWindowEnd = new CalDateTime(windowEnd);

            var occurrences = virtualEvent
                .GetOccurrences()
                .TakeWhileBefore(iCalWindowEnd);

            foreach (var occurrence in occurrences)
            {
                var slotStart = occurrence.Period.StartTime.Value;

                // Ignorar ocurrencias anteriores al comienzo de la ventana.
                if (slotStart < windowStart)
                {
                    continue;
                }

                var slotEnd = slotStart.AddMinutes(durationMinutes);

                slots.Add(new TimeSlot(slotStart, slotEnd));
            }

            return slots;
        }

        public bool ExceedsLimit(string rrule, DateTime date)
        {
            var pattern = new RecurrencePattern(rrule);

            if (pattern.Until == null)
            {
                return false;
            }

            return date.Date > pattern.Until.Value.Date;
        }
        public DateTime GetWindowEnd(
            string rrule,
            DateTime inicio,
            int duracionMinutos)
        {
            var pattern = new RecurrencePattern(rrule);

            if (pattern.Until != null)
            {
                return pattern.Until.Value;
            }

            if (pattern.Count.HasValue)
            {
                var calendarEvent = new CalendarEvent
                {
                    Start = new CalDateTime(inicio),
                    Duration = IcalDuration.FromMinutes(duracionMinutos),
                    RecurrenceRule = pattern
                };

                var occurrences = calendarEvent
                    .GetOccurrences()
                    .Take(pattern.Count.Value)
                    .ToList();

                if (occurrences.Count > 0)
                {
                    var ultima = occurrences.Last().Period.StartTime.Value;

                    return ultima.AddMinutes(duracionMinutos);
                }
            }

            return inicio.AddYears(1);
        }
    }
}