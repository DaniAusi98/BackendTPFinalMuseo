using Core.Domain.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class Personal : DomainEntity<string>
    {
        public string IdentityUserId { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public Telefono Telefono { get; private set; }
        public Email Email { get; private set; }
        public string DNI { get; private set; }
        public DateOnly FechaNacimiento { get; private set; }
        public bool Activo { get; private set; }

        public ICollection<AsignacionPersonal> Asignaciones { get; private set; }
            = new List<AsignacionPersonal>();

        protected Personal()
        {
        }

        public Personal(
            string identityUserId,
            string nombre,
            string apellido,
            string dni,
            DateOnly fechaNacimiento,
            Telefono telefono,
            Email email)
        {
            Id = Guid.NewGuid().ToString();

            SetIdentityUserId(identityUserId);
            SetNombre(nombre);
            SetApellido(apellido);
            SetDNI(dni);
            SetFechaNacimiento(fechaNacimiento);

            Telefono = telefono
                ?? throw new DomainException("Teléfono no puede ser nulo.");

            Email = email
                ?? throw new DomainException("Email no puede ser nulo.");

            Activo = true;
        }

        public void AgregarAsignacion(AsignacionPersonal asignacion)
        {
            if (asignacion is null)
                throw new DomainException("La asignación no puede ser nula.");

            Asignaciones.Add(asignacion);
        }

        public void EliminarAsignacion(string asignacionId)
        {
            var asignacion = Asignaciones
                .FirstOrDefault(x => x.Id == asignacionId);

            if (asignacion is null)
                throw new DomainException("La asignación no existe.");

            Asignaciones.Remove(asignacion);
        }

        public void SetIdentityUserId(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
                throw new DomainException(
                    "IdentityUserId no puede estar vacío.");

            IdentityUserId = identityUserId.Trim();
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException(
                    "Nombre no puede estar vacío.");

            Nombre = nombre.Trim();
        }

        public void SetApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                throw new DomainException(
                    "Apellido no puede estar vacío.");

            Apellido = apellido.Trim();
        }

        public void SetDNI(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new DomainException(
                    "DNI no puede estar vacío.");

            DNI = dni.Trim();
        }

        public void SetFechaNacimiento(DateOnly fechaNacimiento)
        {
            if (fechaNacimiento > DateOnly.FromDateTime(DateTime.UtcNow))
                throw new DomainException(
                    "La fecha de nacimiento no puede ser futura.");

            FechaNacimiento = fechaNacimiento;
        }

        public void SetEstado(bool estado)
        {
            Activo = estado;
        }
    }
}