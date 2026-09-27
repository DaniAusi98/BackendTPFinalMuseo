using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DataTransferObjets;

using Domain.Common.Exceptions;

using Infrastructure.Adapters;

using Microsoft.AspNetCore.Identity;
namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<UsuarioSistema> _userManager;
        private readonly SignInManager<UsuarioSistema> _signInManager;
        private readonly JwtTokenService _jwtTokenService;

        public IdentityService(
            UserManager<UsuarioSistema> userManager, JwtTokenService jwtTokenService,SignInManager<UsuarioSistema> signInManager)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _signInManager = signInManager;

        }
        public async Task<LoginResponseDto?> LoginAsync(string email,string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false
            );

            if (!result.Succeeded)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            var TokenResult = _jwtTokenService.GenerateToken(user, roles);

            return new LoginResponseDto
            {
                User= new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Nombre = user.FirstName,
                    Apellido = user.LastName,
                    Roles = roles.ToList()
                },
                Token = TokenResult.Token,
                ExpiresAt = TokenResult.ExpiresAt

            };
        }


        public async Task<UserDto> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Nombre = user.FirstName,
                Apellido = user.LastName,
                Roles = roles.ToList()
            };

        }

        public async Task<string> RegisterAsync(
            string nombre,
            string apellido,
            DateOnly fechaNac,
            string email,
            string telefono,
            string password)
        {
            var user = new UsuarioSistema(nombre, apellido, fechaNac)
            {
                Email = email,
                UserName = email,
                PhoneNumber = telefono,
                BirthDate = fechaNac
            };

            var result = await _userManager.CreateAsync(
                user,
                password
            );
            if (!result.Succeeded)
            {
                throw new DomainException(
                    string.Join(",", result.Errors.Select(x => x.Description))
                );
            }

            await _userManager.AddToRoleAsync(
                user,
                "Visitante"
            );

            return user.Id;
            ;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user is not null;


        }

        public async Task<UserDto?> FindById(string idUser)
        {
            var user = await _userManager.FindByIdAsync(idUser);
            if (user is null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Nombre = user.FirstName,
                Apellido = user.LastName,
                Roles = roles.ToList()
            };
        }
        public async Task<string>FindEmailById (string idUser)
        {
            var user = await _userManager.FindByIdAsync(idUser);
            if (user is null)
            {
                return null;
            }
            return user.Email;
        }
        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user is null
                ? throw new DomainException("El usuario no existe.")
                : await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        public async Task<bool> ConfirmEmailAsync(string userId,string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(
                user,
                token);

            return result.Succeeded;
        }
    }
}
