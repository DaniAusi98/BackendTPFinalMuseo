using Core.Domain.Entities;
using Domain.Common.Exceptions;

namespace Domain.Common.Entities
{
    public class Departamento : DomainEntity<string>
    {
        public string ProvinciaId { get; private set; }
        public string Nombre { get; private set; }

        public Departamento(string id ,string provinciaId, string nombre)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new DomainException("El ID del departamento es obligatorio.");
            if (string.IsNullOrWhiteSpace(provinciaId))
                throw new DomainException("El ID de la provincia es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre del departamento es obligatorio.");
            Id= id;
            ProvinciaId = provinciaId;
            Nombre = nombre.Trim();
        }

        // Para EF Core
        protected Departamento() { }

        public void CambiarNombre(string nuevoNombre)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new DomainException("El nombre del departamento es obligatorio.");

            Nombre = nuevoNombre.Trim();
        }
    }
}
