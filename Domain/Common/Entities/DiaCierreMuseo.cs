
using Core.Domain.Entities;
using Domain.Common.Exceptions;
using static Domain.Common.Enums.Enums;

namespace Domain.Common.Entities
{
    public class DiaCierreMuseo : DomainEntity<string>
    {
        public DateTime Fecha { get; private set; }

        public DateTime? FechaHasta { get; private set; }

        public MotivoCierreMuseo Motivo { get; private set; }

        public string Observaciones { get; private set; } = string.Empty;

        protected DiaCierreMuseo() { }

        // Cierre de un día completo.
        public DiaCierreMuseo(
            DateTime fecha,
            MotivoCierreMuseo motivo,
            string observaciones = "")
            : this(fecha, null, motivo, observaciones)
        {
        }

        // Cierre con fecha final opcional.
        public DiaCierreMuseo(
            DateTime fecha,
            DateTime? fechaHasta,
            MotivoCierreMuseo motivo,
            string observaciones = "")
        {
            Id = Guid.NewGuid().ToString();

            if (fechaHasta.HasValue && fechaHasta.Value <= fecha)
            {
                throw new DomainException(
                    "La fecha de fin debe ser posterior a la fecha de inicio.");
            }

            if (!Enum.IsDefined(typeof(MotivoCierreMuseo), motivo))
            {
                throw new DomainException(
                    "El motivo de cierre no es válido.");
            }

            if (!string.IsNullOrWhiteSpace(observaciones) &&
                observaciones.Length > 500)
            {
                throw new DomainException(
                    "Las observaciones no pueden superar los 500 caracteres.");
            }

            Fecha = fecha;
            FechaHasta = fechaHasta;
            Motivo = motivo;
            Observaciones = observaciones?.Trim() ?? string.Empty;
        }

        public void ActualizarFechas(
            DateTime nuevaFecha,
            DateTime? nuevaFechaHasta)
        {
            if (nuevaFechaHasta.HasValue &&
                nuevaFechaHasta.Value <= nuevaFecha)
            {
                throw new DomainException(
                    "La fecha de fin debe ser posterior a la fecha de inicio.");
            }

            Fecha = nuevaFecha;
            FechaHasta = nuevaFechaHasta;
        }

        public void ActualizarMotivo(MotivoCierreMuseo nuevoMotivo)
        {
            if (!Enum.IsDefined(typeof(MotivoCierreMuseo), nuevoMotivo))
            {
                throw new DomainException(
                    "El motivo de cierre no es válido.");
            }

            Motivo = nuevoMotivo;
        }

        public void ActualizarObservaciones(string nuevasObservaciones)
        {
            if (!string.IsNullOrWhiteSpace(nuevasObservaciones) &&
                nuevasObservaciones.Length > 500)
            {
                throw new DomainException(
                    "Las observaciones no pueden superar los 500 caracteres.");
            }

            Observaciones = nuevasObservaciones?.Trim() ?? string.Empty;
        }

        public bool SolapaConFechas(DateTime inicio, DateTime fin)
        {
            var fechaFinEfectiva = FechaHasta
                ?? Fecha.Date.AddDays(1);

            return inicio < fechaFinEfectiva && fin > Fecha;
        }
    }
}
