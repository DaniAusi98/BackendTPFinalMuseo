using Core.Domain.Entities;

namespace Domain.ActividadesAreaEducacion.Entities
{
    public class EquipoTrabajoResponsable : DomainEntity<string>
    {
        public string NombreCompleto { get; set; }
        public string InstitucionPerteneciente { get; set; }

        public string ProyectoId { get; set; }
        public ProyectosAreaEducacion Proyecto { get; set; }

        protected EquipoTrabajoResponsable()
        {
        }

        public EquipoTrabajoResponsable(
            string nombreCompleto,
            string institucionPerteneciente)
        {
            ValidarDatos(nombreCompleto, institucionPerteneciente);
            Id = Guid.NewGuid().ToString();
            NombreCompleto = nombreCompleto.Trim();
            InstitucionPerteneciente = institucionPerteneciente.Trim();
        }

        public void Actualizar(
            string nombreCompleto,
            string institucionPerteneciente)
        {
            ValidarDatos(nombreCompleto, institucionPerteneciente);

            NombreCompleto = nombreCompleto.Trim();
            InstitucionPerteneciente = institucionPerteneciente.Trim();
        }

        private static void ValidarDatos(
            string nombreCompleto,
            string institucionPerteneciente)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                throw new ArgumentException(
                    "El nombre completo no puede estar vacío.",
                    nameof(nombreCompleto));

            if (string.IsNullOrWhiteSpace(institucionPerteneciente))
                throw new ArgumentException(
                    "La institución a la que pertenece no puede estar vacía.",
                    nameof(institucionPerteneciente));
        }
    }
}