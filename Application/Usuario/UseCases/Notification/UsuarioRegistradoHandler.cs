/*using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DomainEvents;
using Application.Usuario.Repositories;
using Core.Application;
using Microsoft.Extensions.Logging;

namespace Application.Usuario.UseCases.Notification
{
    internal sealed class UsuarioRegistradoHandler(
        IUsuarioRegistradoEmailSender emailSender,
        IRepositorioUsuarioVisitante usuarioRepo,
        ILogger<UsuarioRegistradoHandler> logger
    ) : IRequestNotificationHandler<UsuarioVisitanteCreado>
    {
        private readonly IUsuarioRegistradoEmailSender _emailSender = emailSender;
        private readonly IRepositorioUsuarioVisitante _usuarioRepo = usuarioRepo;
        private readonly ILogger<UsuarioRegistradoHandler> _logger = logger;

        public async Task Handle(UsuarioVisitanteCreado notification, CancellationToken cancellationToken)
        {
            try
            {
                // 1) Buscar al usuario
                var usuario = await _usuarioRepo.FindOneAsync(notification.UsuarioVisitanteId);
                if (usuario is null)
                {
                    _logger.LogError("No se encontró UsuarioVisitante con Id {UsuarioId}", notification.UsuarioVisitanteId);
                    return;
                }

                // 2) Generar y asignar token al usuario (VO dentro de la entidad)
                var token = Guid.NewGuid().ToString("N");
                var expiration = DateTime.UtcNow.AddHours(24);

                usuario.SetEmailToken(token, expiration);
                _usuarioRepo.Update(usuario.Id,usuario);

                // 3) Armar enlace de confirmación
                var link = $"https://localhost:7204/api/v1/Usuarios/confirmar-email?token={token}";
                var body = $"<p>Confirmá tu correo haciendo click: <a href='{link}'>Confirmar</a></p>";

                // 4) Intentar enviar email hasta 3 veces
                int maxAttempts = 3;
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        await _emailSender.SendEmailAsync(notification.Email, "Confirma tu Email", body);
                        _logger.LogInformation("Email enviado correctamente a {Email} en el intento {Attempt}",
                                               notification.Email, attempt);
                        break; // si funciona, salir del loop
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Intento {Attempt} fallido para enviar email a {Email}", attempt, notification.Email);
                        if (attempt == maxAttempts)
                        {
                            _logger.LogError(ex, "Todos los intentos fallaron para enviar email a {Email}", notification.Email);
                        }
                        else
                        {
                            await Task.Delay(2000, cancellationToken);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando UsuarioVisitanteCreado para UsuarioId {UsuarioId}",
                                 notification.UsuarioVisitanteId);
            }
        }
    }
}
*/
