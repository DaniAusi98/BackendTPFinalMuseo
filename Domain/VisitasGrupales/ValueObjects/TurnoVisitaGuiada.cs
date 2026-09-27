using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;

namespace Domain.VisitasGrupales.ValueObjects
{
    public class TurnoVisitaGuiada : ValueObject
    {
        public TimeOnly HoraInicio { get; private set; }
        public TimeOnly HoraFin { get; private set; }

        public TimeSpan Duracion => HoraFin.ToTimeSpan() - HoraInicio.ToTimeSpan();

        protected TurnoVisitaGuiada() { }

        public TurnoVisitaGuiada(TimeOnly horaInicio, TimeOnly horaFin)
        {
            if (horaFin <= horaInicio)
                throw new DomainException("La hora de fin debe ser posterior a la hora de inicio del turno.");

            var duracion = horaFin.ToTimeSpan() - horaInicio.ToTimeSpan();
            if (duracion.TotalMinutes < 30)
                throw new DomainException("Un turno de visita guiada debe durar al menos 30 minutos.");

            if (duracion.TotalHours > 3)
                throw new DomainException("Un turno de visita guiada no puede durar más de 3 horas.");

            HoraInicio = horaInicio;
            HoraFin = horaFin;
        }

        public bool SeSolapaCon(TurnoVisitaGuiada otro)
        {
            return HoraInicio < otro.HoraFin && HoraFin > otro.HoraInicio;
        }

        public bool EstaEnRango(TimeOnly apertura, TimeOnly cierre)
        {
            return HoraInicio >= apertura && HoraFin <= cierre;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return HoraInicio;
            yield return HoraFin;
        }
    }
}
