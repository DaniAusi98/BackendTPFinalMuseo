using Domain.Common.ValueObjets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Availability.ApplicationServices
{
    
        public interface IRecurrenceEvaluator
        {
            bool ExceedsLimit(string rrule, DateTime date);

            /// <summary>
            /// Genera en memoria todos los bloques de tiempo (slots) de una regla 
            /// que caen dentro de una ventana de observación específica.
            /// </summary>
            /// <param name="rrule">La regla iCalendar desde la BD</param>
            /// <param name="eventStart">La FechaInicio original del evento base</param>
            /// <param name="durationMinutes">La duración base del evento</param>
            /// <param name="windowStart">Inicio del mes/semana a observar</param>
            /// <param name="windowEnd">Fin del mes/semana a observar</param>
            List<TimeSlot> ExpandRule(
                string rrule,
                DateTime eventStart,
                int durationMinutes,
                DateTime windowStart,
                DateTime windowEnd);

            DateTime GetWindowEnd(
                string rrule,
                DateTime eventStart,
                int durationMinutes);
        }
    

}
