
namespace Application.Usuario.ApplicationServices.ApplicationServiceInterfaces
{
    public class IdentityOperationResult
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; } = [];
    }
}
