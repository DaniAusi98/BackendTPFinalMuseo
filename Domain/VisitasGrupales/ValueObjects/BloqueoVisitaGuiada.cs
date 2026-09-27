using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;

namespace Domain.VisitasGrupales.ValueObjects
{
    public class BloqueoVisitasGuiadas : ValueObject
    {
        public DateTime FechaDesde { get; private set; }
        public DateTime FechaHasta { get; private set; }
        public string Motivo { get; private set; }

        public BloqueoVisitasGuiadas()
        {

        }
        public BloqueoVisitasGuiadas(DateTime fechaDesde, DateTime fechaHasta, string motivo)
        {
            if (fechaDesde >= fechaHasta)
                throw new DomainException("La fecha desde debe ser anterior a la fecha hasta.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new DomainException("El motivo del bloqueo es requerido.");

            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
            Motivo = motivo;
        }

        public bool SolapaConFecha(DateTime fecha)
        {
            return fecha.Date >= FechaDesde.Date && fecha.Date < FechaHasta.Date;
        }

        public bool SolapaConPeriodo(DateTime desde, DateTime hasta)
        {
            return FechaDesde < hasta && FechaHasta > desde;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FechaDesde;
            yield return FechaHasta;
            yield return Motivo;
        }
    }
}
