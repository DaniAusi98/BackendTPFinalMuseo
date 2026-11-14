using Application.Common.ApplicationServices;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using System.Net;
using System.Text.RegularExpressions;

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

            // 1. Remitente limpio sin emojis que activen filtros heurísticos
            mensaje.From.Add(new MailboxAddress("Museo Antropología", _mailOptions.UserName));
            mensaje.To.Add(MailboxAddress.Parse(to));
            mensaje.Subject = subject;

            // 2. Oculta el nombre de tu máquina local (@desktop-kamcsp0) usando el dominio del remitente
            var dominio = _mailOptions.UserName.Split('@').LastOrDefault() ?? "gmail.com";
            mensaje.MessageId = MimeUtils.GenerateMessageId(dominio);

            // 3. Incluye versión en texto plano además del HTML
            var cuerpo = new BodyBuilder
            {
                HtmlBody = html,
                TextBody = ConvertirHtmlATextoPlano(html)
            };
            mensaje.Body = cuerpo.ToMessageBody();

            using var client = new SmtpClient();

            // Opcional: Evita que MailKit envíe el nombre de tu PC en el comando EHLO/HELO
            client.LocalDomain = dominio;

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

        private static string ConvertirHtmlATextoPlano(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return string.Empty;

            // Reemplaza saltos de línea HTML por saltos reales y elimina el resto de etiquetas
            var sinSaltos = Regex.Replace(html, @"<(br|p|div|li|tr)[^>]*>", "\n", RegexOptions.IgnoreCase);
            var sinEtiquetas = Regex.Replace(sinSaltos, "<[^>]+>", string.Empty);

            return WebUtility.HtmlDecode(sinEtiquetas).Trim();
        }
    }
}
