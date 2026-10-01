using Core.Domain.Entities;
using Domain.Common.Exceptions;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class Puesto : DomainEntity<string>
    {
        public string Nombre { get; private set; }

        public string Descripcion { get; private set; }
        public bool Activo { get; private set; } = true;
        public Puesto()
        {
        }

        public Puesto(string nombre, string descripcion)
        {
            Id = Guid.NewGuid().ToString();

            SetNombre(nombre);
            SetDescripcion(descripcion);
        }

        private void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DomainException("La descripción del puesto no puede estar vacía.");

            Descripcion = descripcion.Trim();
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre del puesto no puede estar vacío.");

            Nombre = nombre.Trim();
        }
    }
}
