using Domain.Common.ValueObjets;

namespace Domain.Eventos.ValueObjets
{
    public class LogisticaMuestraTemporal : ValueObject
    {
        public DateTime FechaMontaje { get; private set; }
        public DateTime FechaMontajeHasta { get; private set; }
        public DateTime FechaDesmontaje { get; private set; }
        public DateTime FechaDesmontajeHasta { get; private set; }

        protected LogisticaMuestraTemporal()
        {

        }

        public LogisticaMuestraTemporal(DateTime fechaMontaje, DateTime fechaMontajeHasta,
    DateTime fechaDesmontaje, DateTime fechaDesmontajeHasta)
        {
            // 1. Validación: El montaje debe tener un rango válido
            if (fechaMontajeHasta <= fechaMontaje)
            {
                throw new ArgumentException("La fecha de fin de montaje debe ser posterior a la fecha de inicio.");
            }

            // 2. Validación: El desmontaje debe tener un rango válido
            if (fechaDesmontajeHasta <= fechaDesmontaje)
            {
                throw new ArgumentException("La fecha de fin de desmontaje debe ser posterior a la fecha de inicio.");
            }

            // 3. Validación clave: El desmontaje desde debe ser posterior al montaje hasta
            if (fechaDesmontaje < fechaMontajeHasta)
            {
                throw new ArgumentException("El desmontaje no puede comenzar antes de que haya finalizado el montaje.");
            }

            FechaMontaje = fechaMontaje;
            FechaMontajeHasta = fechaMontajeHasta;
            FechaDesmontaje = fechaDesmontaje;
            FechaDesmontajeHasta = fechaDesmontajeHasta;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FechaMontaje;
            yield return FechaMontajeHasta;
            yield return FechaDesmontaje;
            yield return FechaDesmontajeHasta;
        }
    }
}
