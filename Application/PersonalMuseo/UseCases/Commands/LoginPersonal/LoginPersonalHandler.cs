using Application.PersonalMuseo.DataTrasnferObjets;
using Application.PersonalMuseo.Repositories;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DataTransferObjets;
using Core.Application;
using Core.Application.Mapping;
using Domain.Common.Exceptions;

namespace Application.PersonalMuseo.UseCases.Commands.LoginPersonal
{
    internal sealed class LoginPersonalHandler(IIdentityService identityService, IRepositorioPersonal repositorioPersonal) : IRequestCommandHandler<LoginPersonalCommand, LoginPersonalResponseDto>
    {
        private readonly IRepositorioPersonal _repositorioPersonal = repositorioPersonal ?? throw new ArgumentNullException(nameof(repositorioPersonal));

        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));

        public async Task<LoginPersonalResponseDto> Handle(LoginPersonalCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByEmailAsync(request.Email) ?? throw new DomainException("Credenciales inválidas");
            var result = await _identityService.LoginAsync(request.Email, request.Password);
            var personalMuseo = await _repositorioPersonal.GetPersonalMuseo(user.Id);
            var userDto = user.To<UserDto>();
            var personalDto = personalMuseo.To<PersonalDto>();



            //result es un loginresponse 
            /*public class LoginPersonalResponseDto
            {
            public string Token { get; set; } = string.Empty;
            public UserMuseoDto User { get; set; } = default!;
            public DateTime ExpiresAt { get; set; }
            }
              public class UserMuseoDto
            {
                public UserDto UsuarioPersonal { get; set; }
                public PersonalDto Personal { get; set; }

            }*/


            return new LoginPersonalResponseDto
            {
                Token = result.Token,
                User = new UserMuseoDto
                {
                    UsuarioPersonal = userDto,
                    Personal = personalDto

                },
                ExpiresAt = result.ExpiresAt

            };
        }
    }
}
