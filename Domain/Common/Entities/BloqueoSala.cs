using Core.Domain.Entities;
using Domain.Common.Exceptions;
using Domain.RecursoMuseo.Entities;

using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.Common.Enums.Enums;

namespace Domain.Common.Entities
{
    public class BloqueoSala : DomainEntity<string>
    {
        public string SalaId { get; private set; }
        public Sala Sala { get; private set; }

        public DateTime FechaDesde { get; private set; }

        public DateTime FechaHasta { get; private set; }

        public TipoBloqueoSala Motivo { get; private set; }
  
        public string Observaciones { get; private set; } = string.Empty;

        protected BloqueoSala() { }

        public BloqueoSala(
            string salaId,
            DateTime fechaDesde,
            DateTime fechaHasta,
            TipoBloqueoSala motivo,
            string observaciones = "")
        {
            Id = Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(salaId.ToString()))
                throw new DomainException("La sala es obligatoria.");

            if (fechaHasta <= fechaDesde)
                throw new DomainException("La fecha de fin debe ser posterior a la fecha de inicio.");

            if (!Enum.IsDefined(typeof(TipoBloqueoSala), motivo))
                throw new DomainException("El motivo del bloqueo no es válido.");

            if (!string.IsNullOrWhiteSpace(observaciones) &&
                observaciones.Length > 500)
                throw new DomainException("Las observaciones no pueden exceder los 500 caracteres.");

            SalaId = salaId;
            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
            Motivo = motivo;
            Observaciones = observaciones?.Trim() ?? string.Empty;
        }

        public void ActualizarFechas(DateTime nuevaFechaDesde, DateTime nuevaFechaHasta)
        {
            if (nuevaFechaHasta <= nuevaFechaDesde)
                throw new DomainException("La fecha de fin debe ser posterior a la fecha de inicio.");

            FechaDesde = nuevaFechaDesde;
            FechaHasta = nuevaFechaHasta;
        }

        public void ActualizarMotivo(TipoBloqueoSala nuevoMotivo)
        {
            if (!Enum.IsDefined(typeof(TipoBloqueoSala), nuevoMotivo))
                throw new DomainException("El motivo del bloqueo no es válido.");

            Motivo = nuevoMotivo;
        }

        public void ActualizarObservaciones(string nuevasObservaciones)
        {
            if (!string.IsNullOrWhiteSpace(nuevasObservaciones) &&
                nuevasObservaciones.Length > 500)
                throw new DomainException("Las observaciones no pueden exceder los 500 caracteres.");

            Observaciones = nuevasObservaciones?.Trim() ?? string.Empty;
        }
    }
}
