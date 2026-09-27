using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
namespace Infrastructure.Adapters
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        // Genera un hash seguro a partir de la contraseña en texto plano
        public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        // Verifica si una contraseña en texto plano coincide con un hash existente
        public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    }

}
 