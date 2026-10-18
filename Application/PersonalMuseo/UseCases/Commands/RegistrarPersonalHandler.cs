using Application.PersonalMuseo.ApplicationServices;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Core.Application;
namespace Application.PersonalMuseo.UseCases.Commands;

internal sealed class RegistrarPersonalHandler(
    IIdentityService identityService,

    IConfirmUserUrlDashboard confirmUserUrlDashboard,
    IUserConfirmationEmailService userConfirmationEmailService)
    : IRequestCommandHandler<RegistrarPersonalQuery, string>
{
    private readonly IIdentityService _identityService =
        identityService ?? throw new ArgumentNullException(nameof(identityService));



    private readonly IConfirmUserUrlDashboard _confirmUserUrlDashboard =
        confirmUserUrlDashboard ?? throw new ArgumentNullException(nameof(confirmUserUrlDashboard));

    private readonly IUserConfirmationEmailService _userConfirmationEmailService =
        userConfirmationEmailService ?? throw new ArgumentNullException(nameof(userConfirmationEmailService));

    public async Task<string> Handle(
        RegistrarPersonalQuery request,
        CancellationToken cancellationToken)
    {




        var userId = await _identityService.RegistrarPersonalAsync(
            request.Nombre,
            request.Apellido,
            request.Email,
            request.Telefono,
            request.FechaNacimiento,
            request.RolUsuarioId,
            request.Password);


        var token = await _identityService
            .GenerateEmailConfirmationTokenAsync(userId);

        var confirmationUrl =
            _confirmUserUrlDashboard.GetEmailConfirmationUserUrl(
                userId,
                token);

        await _userConfirmationEmailService
            .SendUserConfirmationEmailAsync(
                request.Email,
                confirmationUrl);

        return userId;
    }
}