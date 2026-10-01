using Core.Domain.Entities;
using Domain.Common.Exceptions;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class HorarioLaboral : DomainEntity<string>
    {
        public string PuestoPersonalMuseoId { get; private set; }
        public PuestoPersonalMuseo PuestoPersonalMuseo { get; private set; }

        public DayOfWeek DiaSemana { get; private set; }
        public TimeSpan HoraDesde { get; private set; }
        public TimeSpan HoraHasta { get; private set; }

        protected HorarioLaboral()
        {
        }

        public HorarioLaboral(
            string puestoPersonalMuseoId,
            DayOfWeek diaSemana,
            TimeSpan horaDesde,
            TimeSpan horaHasta)
        {
            Id = Guid.NewGuid().ToString();

            SetPuestoPersonalMuseoId(puestoPersonalMuseoId);
            SetHorario(diaSemana, horaDesde, horaHasta);
        }

        private void SetPuestoPersonalMuseoId(string puestoPersonalMuseoId)
        {
            if (string.IsNullOrWhiteSpace(puestoPersonalMuseoId))
                throw new DomainException(
                    "El identificador del puesto de personal de museo no puede ser vacío.");
            PuestoPersonalMuseoId = puestoPersonalMuseoId.Trim();
        }

        public void SetHorario(
            DayOfWeek diaSemana,
            TimeSpan horaDesde,
            TimeSpan horaHasta)
        {
            if (horaDesde >= horaHasta)
                throw new DomainException(
                    "La hora de inicio debe ser anterior a la hora de fin.");

            DiaSemana = diaSemana;
            HoraDesde = horaDesde;
            HoraHasta = horaHasta;
        }
    }
}
