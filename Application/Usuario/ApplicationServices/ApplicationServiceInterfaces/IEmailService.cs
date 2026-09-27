
namespace Application.Usuario.ApplicationServices.ApplicationServiceInterfaces
{
    public interface IEmailService
    {
        Task SendAsync(
            string to,
            string subject,
            string html,
            CancellationToken cancellationToken = default);
    }
}
