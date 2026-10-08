using Application.PersonalMuseo.ApplicationServices;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Adapters.EmailSender.ResendEmailService.User
{
    public class ConfirmUserUrlDashboard : IConfirmUserUrlDashboard
    {
        private readonly IConfiguration _configuration;

        public ConfirmUserUrlDashboard(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetEmailConfirmationUserUrl(
            string userId,
            string token)
        {
            var frontendUrl = _configuration["Frontend:BaseUrl"];

            return $"{frontendUrl}/confirm-user" + $"?userId={userId}" + $"&token={Uri.EscapeDataString(token)}";
        }
    }

}
