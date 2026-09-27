using Core.Domain.Entities;
using Domain.Common.Exceptions;

namespace Domain.Common.Entities
{
    public class LocalidadArg : DomainEntity<string>
    {
        public string ProvinciaId { get; private set; }
        public string DepartamentoId { get; private set; }
        public string Nombre { get; private set; }

        public LocalidadArg(string id, string departamentoId, string provinciaId, string nombre)
        {
            Id = id;

           if (string.IsNullOrWhiteSpace(provinciaId))
                throw new DomainException("El ID de la provincia es obligatorio.");
           if (string.IsNullOrWhiteSpace(departamentoId))
                throw new DomainException("El ID del departamento es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre de la localidad es obligatorio.");

            ProvinciaId = provinciaId;
            DepartamentoId = departamentoId;
            Nombre = nombre.Trim();
        }

        // Para EF Core
        protected LocalidadArg() { }

        public void CambiarNombre(string nuevoNombre)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new DomainException("El nombre de la localidad es obligatorio.");

            Nombre = nuevoNombre.Trim();
        }
    }
}
