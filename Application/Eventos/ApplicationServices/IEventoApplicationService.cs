using Domain.Common.ValueObjets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Eventos.ApplicationServices
{
    public interface IEventoApplicationService
    {
        /// <summary>
        /// Realiza la validación de resguardo en última instancia utilizando el motor de disponibilidad,
        /// expandiendo las recurrencias propuestas para asegurar que no existan solapamientos concurrentes.
        /// </summary>
        /// <param name="rrulePropuesta">La regla de recurrencia en formato iCalendar enviada desde el frontend</param>
        /// <param name="horarioBasePropuesto">El bloque de tiempo (inicio y fin) de la primera ocurrencia</param>
        /// <param name="salasIdsSolicitadas">La lista de IDs de las salas que se pretenden reservar</param>
        Task ValidarDisponibilidadResguardoAsync(
            string? rrulePropuesta,
            TimeSlot horarioBasePropuesto,
            List<string> salasIdsSolicitadas);
    }
}
