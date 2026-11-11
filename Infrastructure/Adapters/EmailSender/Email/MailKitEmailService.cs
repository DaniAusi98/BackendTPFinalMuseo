using Application.Common.ApplicationServices;     // Tu namespace de IEmailService
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Adapters.EmailSender.Email
{
    public class MailKitEmailService(IOptions<MailOptions> options) : IEmailService
    {
        private readonly MailOptions _mailOptions = options.Value;

        public async Task SendAsync(
            string to,
            string subject,
            string html,
            CancellationToken cancellationToken = default)
        {
            var mensaje = new MimeMessage();

            mensaje.From.Add(new MailboxAddress("🏛️ Museo Antropología", _mailOptions.UserName));
            mensaje.To.Add(MailboxAddress.Parse(to));
            mensaje.Subject = subject;

            var cuerpo = new BodyBuilder { HtmlBody = html };
            mensaje.Body = cuerpo.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(
                _mailOptions.Host,
                _mailOptions.Port,
                MailKit.Security.SecureSocketOptions.StartTls,
                cancellationToken
            );

            await client.AuthenticateAsync(_mailOptions.UserName, _mailOptions.Password, cancellationToken);

            await client.SendAsync(mensaje, cancellationToken);

            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}
