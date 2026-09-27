using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;

namespace Application.Usuario.UseCases.Commands.UpdateUsuario
{
    public class ConfirmEmailServiceResponse : IConfirmEmailService
    {
        private readonly IIdentityService _identityService;

        public ConfirmEmailServiceResponse(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> ExecuteAsync(
            string userId,
            string token,
            CancellationToken cancellationToken = default)
        {
            return await _identityService.ConfirmEmailAsync(
            userId,
            token);
        }
    }
}
