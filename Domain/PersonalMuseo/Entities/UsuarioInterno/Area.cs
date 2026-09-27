using Core.Domain.Entities;
using Domain.Common.Exceptions;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class Area : DomainEntity<string>
    {
        public string Nombre { get; private set; }
        public List<AreaPuesto> AreaPuestos { get; private set; } = new List<AreaPuesto>();

        public Area()
        {
        }

        public Area(string nombre)
        {
            Id = Guid.NewGuid().ToString();

            SetNombre(nombre);
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre del área no puede estar vacío.");

            Nombre = nombre.Trim();
        }
    }
}
