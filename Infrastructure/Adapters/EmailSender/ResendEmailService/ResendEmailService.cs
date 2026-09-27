using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;

using Resend;
namespace Infrastructure.Adapters.EmailSender.ResendEmailService
{
    public class ResendEmailService(IResend resend) : IEmailService
    {
        private readonly IResend _resend = resend;

        public async Task SendAsync(
            string to,
            string subject,
            string html,
            CancellationToken cancellationToken = default)
        {
            var message = new EmailMessage
            {
                From = "onboarding@resend.dev",
                To = to,
                Subject = subject,
                HtmlBody = html
            };

            await _resend.EmailSendAsync(message, cancellationToken);
        }


    }
}
