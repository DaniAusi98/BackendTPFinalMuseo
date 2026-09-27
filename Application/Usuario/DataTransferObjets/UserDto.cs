
namespace Application.Usuario.DataTransferObjets
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public List<string> Roles { get; set; } = [];
        public UserDto() { }
    }

}
