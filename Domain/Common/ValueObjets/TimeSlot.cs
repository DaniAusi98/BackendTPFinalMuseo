using Domain.Common.Exceptions;
namespace Domain.Common.ValueObjets
{
    public class TimeSlot : ValueObject
    {
        public DateTime Inicio { get;private set; }

        public DateTime Fin { get;private set; }

        protected TimeSlot() { } // EF

        public TimeSlot(DateTime inicio, DateTime fin)
        {
            if (fin <= inicio)
                throw new DomainException("La hora de fin debe ser posterior a la hora de inicio.");
            Inicio = inicio;
            Fin = fin;
        }

        public bool SeSolapaCon(TimeSlot otro)
        {
            return Inicio < otro.Fin && Fin > otro.Inicio;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Inicio;
            yield return Fin;
        }
    }
}
