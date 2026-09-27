using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class UsuarioSistema : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }

        private UsuarioSistema()
        {

        }
        public UsuarioSistema(string nombre, string apellido, DateOnly fechaNac)
        {
            FirstName = nombre ?? throw new ArgumentNullException(nameof(nombre));
            LastName = apellido ?? throw new ArgumentNullException(nameof(apellido));
            BirthDate = fechaNac;
        }
        public UsuarioSistema(string id, string nombre, string apellido, DateOnly fechaNac)
           : this(nombre, apellido, fechaNac)
        {
            Id = id;
        }


    }
}
