using Application.VisitaGrupal.ApplicationServices;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Adapters.EmailSender.ResendEmailService.ConfirmarVisitaUrl
{
    internal class UrlConfirmacionVisitaGrupal(IConfiguration configuration) : IURLConfirmacionVisitaGrupal
    {
        private readonly IConfiguration _configuration = configuration;

        public string GetUrlConfirmacionVisitaGrupal(string visitaId)
        {
            var frontendUrl = _configuration["Frontend:BaseUrl"];
            return $"{frontendUrl}/confirmar-visita-grupal?visitaId={visitaId}";
        }
    }
}
