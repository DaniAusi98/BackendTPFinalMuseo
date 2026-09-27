using Core.Domain.Entities;

namespace Domain.ActividadesAreaEducacion.Entities
{
    public class ProyectosAreaEducacion : DomainEntity<string>
    {
        public string Nombre { get; private set; }
        public DateTime FechaInicio { get; private set; }
        public DateTime FechaFin { get; private set; }
        public List<EquipoTrabajoResponsable> EquipoTrabajoResponsable { get; private set; } = new();
        public string Publico { get; private set; }

        protected ProyectosAreaEducacion()
        {
        }

        public ProyectosAreaEducacion(
            string nombre,
            DateTime fechaInicio,
            DateTime fechaFin,
            List<EquipoTrabajoResponsable> equipoTrabajoResponsable,
            string publico)
        {
            ValidarDatos(
                nombre,
                fechaInicio,
                fechaFin,
                equipoTrabajoResponsable,
                publico);
            Id = Guid.NewGuid().ToString();

            Nombre = nombre.Trim();
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            EquipoTrabajoResponsable = equipoTrabajoResponsable;
            Publico = publico.Trim();
        }

        public void ActualizarProyecto(
            string nombre,
            DateTime fechaInicio,
            DateTime fechaFin,
            List<EquipoTrabajoResponsable> equipoTrabajoResponsables,
            string publico)
        {
            ValidarDatos(
                nombre,
                fechaInicio,
                fechaFin,
                equipoTrabajoResponsables,
                publico);
            Nombre = nombre.Trim();
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            EquipoTrabajoResponsable = equipoTrabajoResponsables;
            Publico = publico.Trim();
        }

        private static void ValidarDatos(
            string nombre,
            DateTime fechaInicio,
            DateTime fechaFin,
            List<EquipoTrabajoResponsable> equipoTrabajoResponsables,
            string publico)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException(
                    "El nombre del proyecto no puede estar vacío.",
                    nameof(nombre));

            if (fechaInicio == default)
                throw new ArgumentException(
                    "La fecha de inicio es obligatoria.",
                    nameof(fechaInicio));

            if (fechaFin == default)
                throw new ArgumentException(
                    "La fecha de fin es obligatoria.",
                    nameof(fechaFin));

            if (fechaFin < fechaInicio)
                throw new ArgumentException(
                    "La fecha de fin no puede ser anterior a la fecha de inicio.",
                    nameof(fechaFin));

            if (equipoTrabajoResponsables == null)
                throw new ArgumentNullException(
                    nameof(equipoTrabajoResponsables),
                    "El equipo de trabajo responsable no puede ser nulo.");

            if (equipoTrabajoResponsables.Count == 0)
                throw new ArgumentException(
                    "El proyecto debe tener al menos un responsable.",
                    nameof(equipoTrabajoResponsables));

            if (string.IsNullOrWhiteSpace(publico))
                throw new ArgumentException(
                    "El público no puede estar vacío.",
                    nameof(publico));
        }
    }
}