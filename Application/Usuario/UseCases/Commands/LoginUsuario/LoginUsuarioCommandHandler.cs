using Application.Usuario.ApplicationServices;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DataTransferObjets;

using AutoMapper;

using Core.Application;

using Domain.Common.Exceptions;

namespace Application.Usuario.UseCases.Commands.LoginUsuario
{
    internal sealed class LoginUsuarioCommandHandler(IIdentityService identityService) : IRequestCommandHandler<LoginUsuarioCommand, LoginResponseDto>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));


        public async Task<LoginResponseDto> Handle(LoginUsuarioCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByEmailAsync(request.Email) ?? throw new DomainException("Credenciales inválidas");

            var result = await _identityService.LoginAsync(
            request.Email,
            request.Password
            );

            return result is null ? throw new DomainException("Email o contraseña incorrectos") : result;
        }


    }
}

