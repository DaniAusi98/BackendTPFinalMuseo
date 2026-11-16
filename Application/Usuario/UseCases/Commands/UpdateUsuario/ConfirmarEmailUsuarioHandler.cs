using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Core.Application;

namespace Application.Usuario.UseCases.Commands.UpdateUsuario
{
    internal class ConfirmarEmailUsuarioHandler(IConfirmEmailService confirmEmailService) : IRequestCommandHandler<ConfirmEmailRequest, bool>
    {
        private IConfirmEmailService _confirmEmailService = confirmEmailService ?? throw new ArgumentNullException(nameof(confirmEmailService));
        public async Task<bool> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var result = await _confirmEmailService.ExecuteAsync(request.UserId, request.Token);

            return result;
        }
    }
}
