using Application.VisitaGrupal.ApplicationServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Adapters.EmailSender.ResendEmailService.ConfirmarVisitaUrl
{
    internal class UrlConfirmacionVisitaGrupal : IURLConfirmacionVisitaGrupal
    {
        private readonly IConfiguration _configuration;
        public UrlConfirmacionVisitaGrupal(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetUrlConfirmacionVisitaGrupal(string visitaId)
        {
            var frontendUrl = _configuration["Frontend:BaseUrl"];
            return $"{frontendUrl}/confirmar-visita-grupal?visitaId={visitaId}";
        }
    }
}
