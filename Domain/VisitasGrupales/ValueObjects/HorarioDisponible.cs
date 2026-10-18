using Domain.Common.ValueObjets;

namespace Domain.VisitasGrupales.ValueObjects
{
    public class HorarioDisponible : ValueObject
    {
        public DayOfWeek Dia { get; private set; }
        public TimeOnly Desde { get; private set; }
        public TimeOnly Hasta { get; private set; }

        private HorarioDisponible() { }

        public HorarioDisponible(DayOfWeek dayOfWeek, TimeOnly desde, TimeOnly hasta)
        {
            if (hasta <= desde)
                throw new ArgumentException("La hora de fin (Hasta) debe ser posterior a la de inicio (Desde).");

            Dia = dayOfWeek;
            Desde = desde;
            Hasta = hasta;
        }

        // 1. ¿Un DateTime específico cae dentro de este horario?
        public bool Contiene(DateTime fechaHora)
        {
            if (fechaHora.DayOfWeek != Dia)
                return false;

            TimeOnly horaActual = TimeOnly.FromDateTime(fechaHora);
            return horaActual >= Desde && horaActual <= Hasta;
        }

        // 2. ¿Un intervalo completo (Inicio/Fin) cabe perfectamente aquí adentro?
        public bool ContieneIntervalo(DateTime inicio, DateTime fin)
        {
            if (inicio.Date != fin.Date)
                return false; // No manejamos intervalos que cruzan la medianoche en este objeto

            if (inicio.DayOfWeek != Dia)
                return false;

            TimeOnly horaInicio = TimeOnly.FromDateTime(inicio);
            TimeOnly horaFin = TimeOnly.FromDateTime(fin);

            return horaInicio >= Desde && horaFin <= Hasta;
        }

        // Implementación obligatoria para que el Value Object funcione correctamente
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Dia;
            yield return Desde;
            yield return Hasta;
        }
    }
}
