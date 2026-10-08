using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.PersonalMuseo.UseCases.Commands
{
    public class RegistrarPersonalQuery : IRequestCommand<string>
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "El DNI es obligatorio")]
        public string DNI { get; set; }
        [Required(ErrorMessage = "El email es obligatorio")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateOnly FechaNacimiento { get; set; }
        [Required(ErrorMessage = "Al menos un área o puesto es obligatorio")]
        public string AreaPuestoId { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]

        public string RolUsuarioId { get; set; }
        public string Password { get; set; }

    }
}

/*public string IdentityUserId { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string DNI { get; private set; }
        public DateTime FechaNacimiento { get; private set; }
        public bool Activo { get; private set; } 
        public string AreaPuestoId { get; private set; }








*/