using Core.Domain.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;

namespace Domain.ActividadMuseo.Entities
{
    public class ActividadReprogramada : DomainEntity<string>
    {

        public string ActividadOriginalId { get; private set; }
        public string ActividadNuevaId { get; private set; }

        public TimeSlot HorarioAnterior { get; private set; }
        public TimeSlot HorarioNuevo { get; private set; }

        public ActividadReprogramada(string idActividadOriginal, string idActividadNueva, TimeSlot horarioAnterior, TimeSlot horarioNuevo)
        {
            if (string.IsNullOrEmpty(idActividadOriginal))
                throw new ArgumentNullException("El id de la actividad reprogramada no puede estar vacío.");
            if (string.IsNullOrEmpty(idActividadNueva))
                throw new ArgumentNullException("El id de la nueva actividad  no puede estar vacío.");
            if (horarioAnterior is null)
                throw new DomainException("El horario no puede ser nulo.");
            if (horarioNuevo is null)
                throw new DomainException("El horario no puede ser nulo.");

            Id = Guid.NewGuid().ToString();
            ActividadOriginalId = idActividadOriginal;
            ActividadNuevaId = idActividadNueva;
            HorarioAnterior = horarioAnterior;
            HorarioNuevo = horarioNuevo;

        }
    }

}
