using Core.Domain.Entities;
using Domain.Common.Exceptions;

namespace Domain.Common.Entities
{
    public class Provincia : DomainEntity<string>
    {
        public string Nombre { get; private set; }

        public Provincia(string id,string nombre)
        {
            Id = id;
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre de la provincia es obligatorio.");

            Nombre = nombre.Trim();
        }

        // Para EF Core
        protected Provincia() { }

        public void CambiarNombre(string nuevoNombre)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new DomainException("El nombre de la provincia es obligatorio.");

            Nombre = nuevoNombre.Trim();
        }
    }
}
