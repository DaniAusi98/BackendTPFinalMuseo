using Application.PersonalMuseo.ApplicationServices;
using Application.PersonalMuseo.Repositories;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Core.Application;
using Domain.Common.ValueObjets;
using Domain.PersonalMuseo.Entities.UsuarioInterno;
namespace Application.PersonalMuseo.UseCases.Commands;

internal sealed class RegistrarPersonalHandler(
    IIdentityService identityService,
    IRepositorioPersonal repositorioPersonal,
    IRepositorioAreaPuesto repositorioAreaPuesto,
    IRepositorioAsignacionPuestoPersonal repositorioAsignacion,
    IConfirmUserUrlDashboard confirmUserUrlDashboard,
    IUserConfirmationEmailService userConfirmationEmailService)
    : IRequestCommandHandler<RegistrarPersonalQuery, string>
{
    private readonly IIdentityService _identityService =
        identityService ?? throw new ArgumentNullException(nameof(identityService));

    private readonly IRepositorioPersonal _repositorioPersonal =
        repositorioPersonal ?? throw new ArgumentNullException(nameof(repositorioPersonal));

    private readonly IRepositorioAreaPuesto _repositorioAreaPuesto =
        repositorioAreaPuesto ?? throw new ArgumentNullException(nameof(repositorioAreaPuesto));

    private readonly IRepositorioAsignacionPuestoPersonal _repositorioAsignacion =
        repositorioAsignacion ?? throw new ArgumentNullException(nameof(repositorioAsignacion));

    private readonly IConfirmUserUrlDashboard _confirmUserUrlDashboard =
        confirmUserUrlDashboard ?? throw new ArgumentNullException(nameof(confirmUserUrlDashboard));

    private readonly IUserConfirmationEmailService _userConfirmationEmailService =
        userConfirmationEmailService ?? throw new ArgumentNullException(nameof(userConfirmationEmailService));

    public async Task<string> Handle(
        RegistrarPersonalQuery request,
        CancellationToken cancellationToken)
    {
        var areaPuesto = await _repositorioAreaPuesto
            .FindOneAsync(request.AreaPuestoId);

        if (areaPuesto is null)
            throw new InvalidOperationException(
                "El área y puesto seleccionado no existe.");

        var userId = await _identityService.RegistrarPersonalAsync(
            request.Nombre,
            request.Apellido,
            request.Email,
            request.Telefono,
            request.FechaNacimiento,
            request.RolUsuarioId,
            request.Password);

        var email = new Email(request.Email);
        var telefono = new Telefono(request.Telefono);

        var personal = new Personal(
            userId,
            request.Nombre,
            request.Apellido,
            request.DNI,
            request.FechaNacimiento,
            telefono,
            email);

        await _repositorioPersonal.AddAsync(personal);

        var asignacion = new AsignacionPersonal(
            personal.Id,
            areaPuesto.Id);

        await _repositorioAsignacion.AddAsync(asignacion);

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

        return personal.Id;
    }
}