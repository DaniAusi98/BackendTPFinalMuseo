
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Adapters.EmailSender.ResendEmailService.User
{
    public class ConfirmUserUrl : IConfirmUserUrl
    {
        private readonly IConfiguration _configuration;

        public ConfirmUserUrl(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetEmailConfirmationUrl(
            string userId,
            string token)
        {
            var frontendUrl = _configuration["Frontend:BaseUrl"];

            return $"{frontendUrl}/confirm-email"+$"?userId={userId}"+$"&token={Uri.EscapeDataString(token)}";
        }
    }
}
