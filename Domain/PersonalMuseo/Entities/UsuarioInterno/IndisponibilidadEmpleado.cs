using Core.Domain.Entities;
using Domain.Common.Exceptions;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class IndisponibilidadEmpleado : DomainEntity<string>
    {
        public string PersonalMuseoId { get; private set; }
        public Personal PersonalMuseo { get; private set; } = default!;

        public DateTime FechaDesde { get; private set; }
        public DateTime FechaHasta { get; private set; }
        public string Motivo { get; private set; }

        private IndisponibilidadEmpleado() { }

        public IndisponibilidadEmpleado(
            DateTime fechaDesde,
            DateTime fechaHasta,
            string motivo)
        {
            Id = Guid.NewGuid().ToString();

            if (fechaHasta < fechaDesde)
                throw new DomainException(
                    "La fecha hasta no puede ser menor a la fecha desde.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new DomainException(
                    "El motivo de la ausencia no puede ser vacío.");

            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
            Motivo = motivo.Trim();
        }

        public void ActualizarFechas(
            DateTime nuevaFechaDesde,
            DateTime nuevaFechaHasta)
        {
            if (nuevaFechaHasta < nuevaFechaDesde)
                throw new DomainException(
                    "La fecha hasta no puede ser menor a la fecha desde.");

            FechaDesde = nuevaFechaDesde;
            FechaHasta = nuevaFechaHasta;
        }
    }
}
