/*using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.Usuario.Repositories;
using Core.Application;
namespace Application.Usuario.UseCases.Commands.UpdateUsuario
{
    internal sealed class ConfirmarEmailHandler(
       ICommandQueryBus domainBus,
       IRepositorioUsuarioVisitante usuarioRepo
   ) : IRequestCommandHandler<ConfirmarEmailUsuarioCommand, string>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioUsuarioVisitante _usuarioRepo = usuarioRepo ?? throw new ArgumentNullException(nameof(usuarioRepo));

        public async Task<string> Handle(ConfirmarEmailUsuarioCommand request, CancellationToken cancellationToken)
        {
            // Buscar al usuario por token
            var usuario = await _usuarioRepo.FindByTokenAsync(request.Token);
            if (usuario is null)
                return "Token inválido.";


            try
            {
                // Validar el token usando la lógica de la entidad y VO
                usuario.ValidateToken(request.Token);

                // Confirmar email
                usuario.ConfirmarEmail();
                _usuarioRepo.Update(usuario.Id,usuario);

                return "Email confirmado correctamente";
            }
            catch (InvalidOperationException ex)
            {
                // Si el token no coincide o expiró, devolvemos mensaje al endpoint
                return $"Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                // Otro tipo de error
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex);
            }
        }
    }
}
*/
