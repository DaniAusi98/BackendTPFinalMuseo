using Application.PersonalMuseo.ApplicationServices;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Adapters.EmailSender.ResendEmailService.User
{
    public class ConfirmChangeEmailUrlDashboard(IConfiguration configuration) : IConfirmChangeEmailUrlDashboard
    {
        private readonly IConfiguration _configuration = configuration;

        public string GetChangeEmailConfirmationUrl(
            string userId,
            string nuevoEmail,
            string token)
        {
            var frontendUrl = _configuration["Frontend:BaseUrl"];

            return $"{frontendUrl}/confirm-change-email" +
                   $"?userId={Uri.EscapeDataString(userId)}" +
                   $"&email={Uri.EscapeDataString(nuevoEmail)}" +
                   $"&token={Uri.EscapeDataString(token)}";
        }
    }
}