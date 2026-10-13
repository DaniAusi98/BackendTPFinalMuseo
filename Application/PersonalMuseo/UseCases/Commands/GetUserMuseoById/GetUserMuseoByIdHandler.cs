using Application.PersonalMuseo.DataTrasnferObjets;
using Application.PersonalMuseo.Repositories;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DataTransferObjets;
using Core.Application;
using Core.Application.Mapping;

namespace Application.PersonalMuseo.UseCases.Commands.GetUserMuseoById
{
    internal class GetUserMuseoByIdHandler(IIdentityService identityService, IRepositorioPersonal repositorioPersonal) : IRequestQueryHandler<GetUserMuseoByIdQuery, UserMuseoDto>
    {
        private readonly IRepositorioPersonal _repositorioPersonal = repositorioPersonal;
        private readonly IIdentityService _identityService = identityService;
        public async Task<UserMuseoDto> Handle(GetUserMuseoByIdQuery request, CancellationToken cancellationToken)
        {
            var personalMuseo = await _repositorioPersonal.GetPersonalMuseo(request.UserMuseoId);
            var personalDto = personalMuseo.To<PersonalDto>();
            var user = await _identityService.FindById(request.UserMuseoId);
            var userDto = user.To<UserDto>();

            return new UserMuseoDto
            {
                UsuarioPersonal = userDto,
                Personal = personalDto

            };

        }
    }
}
