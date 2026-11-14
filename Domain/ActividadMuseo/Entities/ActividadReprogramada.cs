
using Core.Domain.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;

namespace Domain.ActividadMuseo.Entities
{
    public class ActividadReprogramada : DomainEntity<string>
    {
        public string ActividadId { get; private set; }
        public ActividadBase Actividad { get; private set; }

        public TimeSlot HorarioAnterior { get; private set; }
        public TimeSlot HorarioNuevo { get; private set; }

        public DateTime FechaReprogramacion { get; private set; }

        public string? Motivo { get; private set; }

        public string? UsuarioId { get; private set; }

        private ActividadReprogramada() { }

        public ActividadReprogramada(
            string actividadId,
            TimeSlot horarioAnterior,
            TimeSlot horarioNuevo,
            string? motivo = null,
            string? usuarioId = null)
        {
            if (string.IsNullOrWhiteSpace(actividadId))
                throw new DomainException("El id de la actividad no puede estar vacío.");

            if (horarioAnterior is null)
                throw new DomainException("El horario anterior no puede ser nulo.");

            if (horarioNuevo is null)
                throw new DomainException("El horario nuevo no puede ser nulo.");

            if (horarioAnterior.Equals(horarioNuevo))
                throw new DomainException("El nuevo horario debe ser diferente del anterior.");

            Id = Guid.NewGuid().ToString();

            ActividadId = actividadId;
            HorarioAnterior = horarioAnterior;
            HorarioNuevo = horarioNuevo;

            FechaReprogramacion = DateTime.UtcNow;

            Motivo = string.IsNullOrWhiteSpace(motivo)
                ? null
                : motivo.Trim();

            UsuarioId = string.IsNullOrWhiteSpace(usuarioId)
                ? null
                : usuarioId;
        }
    }
}
