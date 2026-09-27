using Core.Domain.Entities;

using Domain.RecursoMuseo.Entities;
using Domain.Common.Exceptions;
using Domain.RecursoMuseo.Enums;

using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.Common.Enums.Enums;

namespace Domain.Common.Entities
{
    public class DiaCierreMuseo : DomainEntity<string>
    {
        public DateTime FechaDesde { get; private set; }

        public DateTime FechaHasta { get; private set; }

        public MotivoCierreMuseo Motivo { get; private set; }

        public string Observaciones { get; private set; } = string.Empty;
        protected DiaCierreMuseo() { }

        public DiaCierreMuseo(
            DateTime fecha,
            DateTime fechaHasta,
            MotivoCierreMuseo motivo,
            string observaciones = "")
        {
            Id = Guid.NewGuid().ToString();

            if (fechaHasta <= fecha)
                throw new DomainException("La fecha de fin debe ser posterior a la fecha de inicio.");

            if (!Enum.IsDefined(typeof(MotivoCierreMuseo), motivo))
                throw new DomainException("El motivo de cierre no es válido.");

            if (!string.IsNullOrWhiteSpace(observaciones) &&
                observaciones.Length > 500)
                throw new DomainException("Las observaciones no pueden superar los 500 caracteres.");

            FechaDesde = fecha;
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

        public void ActualizarMotivo(MotivoCierreMuseo nuevoMotivo)
        {
            if (!Enum.IsDefined(typeof(MotivoCierreMuseo), nuevoMotivo))
                throw new DomainException("El motivo de cierre no es válido.");

            Motivo = nuevoMotivo;
        }

        public void ActualizarObservaciones(string nuevasObservaciones)
        {
            if (!string.IsNullOrWhiteSpace(nuevasObservaciones) &&
                nuevasObservaciones.Length > 500)
                throw new DomainException("Las observaciones no pueden superar los 500 caracteres.");

            Observaciones = nuevasObservaciones?.Trim() ?? string.Empty;
        }

        public bool SolapaConFechas(DateTime inicio, DateTime fin)
        {
            return inicio < FechaHasta && fin > FechaDesde;
        }
    }
}
