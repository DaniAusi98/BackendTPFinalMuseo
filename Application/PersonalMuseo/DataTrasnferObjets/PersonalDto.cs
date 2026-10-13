using Domain.Common.ValueObjets;

namespace Application.PersonalMuseo.DataTrasnferObjets
{
    public class PersonalDto
    {
        public string Id { get; private set; }
        public string IdentityUserId { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public Telefono Telefono { get; private set; }
        public Email Email { get; private set; }
        public string DNI { get; private set; }
        public DateOnly FechaNacimiento { get; private set; }
        public bool Activo { get; private set; }
        public IList<AsignacionPersonalDto> Asignaciones { get; private set; }

    }
}
