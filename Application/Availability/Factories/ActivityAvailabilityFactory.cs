using Application.Availability.ApplicationServices;
using Application.Availability.Models;
using Domain.Common.ValueObjets;


namespace Application.Availability.Factories
{
    public class ActivityAvailabilityFactory(IRecurrenceEvaluator recurrenceEvaluator)
    {
        private readonly IRecurrenceEvaluator _recurrenceEvaluator = recurrenceEvaluator;

        public IReadOnlyCollection<ActivityAvailabilityEntry> Create(
            IEnumerable<Domain.ActividadMuseo.Entities.ActividadMuseo> actividades,
            DateTime windowStart,
            DateTime windowEnd)
        {
            var result = new List<ActivityAvailabilityEntry>();

            foreach (var actividad in actividades)
            {
                List<TimeSlot> timeSlots;

                if (!actividad.EsRecurrencia)
                {
                    // Si no es recurrente, el único slot válido es su horario base
                    timeSlots = new List<TimeSlot> { actividad.Horario };
                }
                else
                {
                    // 1. Calculamos la duración base original en minutos crudos
                    int duracionMinutos = (int)(actividad.Horario.Fin - actividad.Horario.Inicio).TotalMinutes;
                    // 2. Expandimos todas las ocurrencias matemáticas usando el evaluador de infraestructura
                    List<TimeSlot> todosLosSlotsCalculados = _recurrenceEvaluator.ExpandRule(
                        actividad.RRule!,
                        actividad.Horario.Inicio,
                        duracionMinutos,
                        windowStart,
                        windowEnd
                    );

                    // 3. Extraemos las fechas de excepción para filtrarlas en memoria (Haciendo match solo por la fecha)
                    var fechasAExcluir = actividad.Exceptions
                        .Select(e => e.FechaExcluir.Date)
                        .ToHashSet();

                    // 4. Descartamos los slots que coincidan con las cancelaciones registradas
                    timeSlots = [.. todosLosSlotsCalculados.Where(slot => !fechasAExcluir.Contains(slot.Inicio.Date))];
                }

                result.Add(new ActivityAvailabilityEntry(actividad, timeSlots));
            }

            return result;
        }
    }
}
