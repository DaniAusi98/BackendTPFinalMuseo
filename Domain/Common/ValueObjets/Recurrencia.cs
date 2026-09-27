using Domain.Common.Exceptions;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.Common.Enums.Enums;

namespace Domain.Common.ValueObjets
{
    public class Recurrencia : ValueObject
    {
        public Frecuencia Tipo { get; }
        public DateOnly FechaInicio { get; }
        public DateOnly FechaFin { get; }
        public TimeOnly HoraInicio { get; }
        public TimeOnly HoraFin { get; }
        public IReadOnlyCollection<DayOfWeek> DiasDeSemana { get; }

        public Recurrencia(
            Frecuencia tipo,
            DateOnly fechaInicio,
            DateOnly fechaFin,
            TimeOnly horaInicio,
            TimeOnly horaFin,
            IEnumerable<DayOfWeek> diasDeSemana = null)
        {
            if (fechaInicio > fechaFin)
                throw new DomainException("La fecha inicio no puede ser mayor a fecha fin.");

            if (horaInicio >= horaFin)
                throw new DomainException("La hora inicio debe ser menor a la hora fin.");

            if (tipo == Frecuencia.Semanal &&
                (diasDeSemana == null || !diasDeSemana.Any()))
                throw new DomainException("Debe incluir días de la semana para la frecuencia semanal.");
            Tipo = tipo;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            HoraInicio = horaInicio;
            HoraFin = horaFin;

            DiasDeSemana = diasDeSemana?.ToList().AsReadOnly()
                            ?? new List<DayOfWeek>().AsReadOnly();
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Tipo;
            yield return FechaInicio;
            yield return FechaFin;
            yield return HoraInicio;
            yield return HoraFin;

            // Enumerable: cada día debe formar parte de la igualdad
            foreach (var dia in DiasDeSemana)
                yield return dia;

        }
    }
}
