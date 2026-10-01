using Core.Domain.Entities;
using Domain.Common.Exceptions;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class Personal : DomainEntity<string>
    {
        public string IdentityUserId { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string DNI { get; private set; }
        public DateTime FechaNacimiento { get; private set; }
        public DateTime FechaIngreso { get; private set; }
        public DateTime? FechaBaja { get; private set; }
        public bool Estado { get; private set; }

        public ICollection<PuestoPersonalMuseo> Asignaciones { get; private set; }
       = new List<PuestoPersonalMuseo>();

        protected Personal()
        {
        }

        public Personal(string identityUserId, string nombre, string apellido, string dNI, DateTime fechaNacimiento, DateTime fechaIngreso)
        {
            Id = Guid.NewGuid().ToString();
            SetIdentityUserId(identityUserId);
            SetNombre(nombre);
            SetApellido(apellido);
            SetDNI(dNI);
            SetFechaNacimiento(fechaNacimiento);
            SetFechaIngreso(fechaIngreso);
        }

        public void SetIdentityUserId(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
                throw new DomainException("IdentityUserId no puede estar vacío.");

            IdentityUserId = identityUserId.Trim();
        }
        public void AgregarAsignacion(PuestoPersonalMuseo asignacion)
        {
            if (asignacion == null)
                throw new DomainException("La asignación no puede ser nula.");

            Asignaciones.Add(asignacion);
        }

        public void EliminarAsignacion(string asignacionId)
        {
            var asignacion = Asignaciones.FirstOrDefault(x => x.Id == asignacionId);

            if (asignacion == null)
                throw new DomainException("La asignación no existe.");

            Asignaciones.Remove(asignacion);
        }

        public void ModificarAsignacion(PuestoPersonalMuseo asignacion)
        {
            if (asignacion == null)
                throw new DomainException("La asignación no puede ser nula.");

            var existente = Asignaciones.FirstOrDefault(x => x.Id == asignacion.Id);

            if (existente == null)
                throw new DomainException("La asignación no existe.");

            existente.Modificar(
                asignacion.AreaPuestoId,
                asignacion.FechaDesde,
                asignacion.FechaHasta);
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("Nombre no puede estar vacío.");
            Nombre = nombre.Trim();
        }
        public void SetApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                throw new DomainException("Apellido no puede estar vacío.");
            Apellido = apellido.Trim();
        }
        public void SetDNI(string dNI)
        {
            if (string.IsNullOrWhiteSpace(dNI))
                throw new DomainException("DNI no puede estar vacío.");
            DNI = dNI.Trim();
        }
        public void SetFechaNacimiento(DateTime fechaNacimiento)
        {
            if (fechaNacimiento > DateTime.Now)
                throw new DomainException("Fecha de nacimiento no puede ser en el futuro.");
            FechaNacimiento = fechaNacimiento;
        }
        public void SetFechaIngreso(DateTime fechaIngreso)
        {
            if (fechaIngreso > DateTime.Now)
                throw new DomainException("Fecha de ingreso no puede ser en el futuro.");
            FechaIngreso = fechaIngreso;
        }
        public void SetFechaBaja(DateTime? fechaBaja)
        {
            if (fechaBaja.HasValue && fechaBaja.Value < FechaIngreso)
                throw new DomainException("Fecha de baja no puede ser anterior a la fecha de ingreso.");
            FechaBaja = fechaBaja;
        }
        public void SetEstado(bool estado)
        {
            Estado = estado;
        }
    }
}