using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DataTransferObjets;
using Domain.Common.Exceptions;
using Infrastructure.Adapters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<UsuarioSistema> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<UsuarioSistema> _signInManager;
        private readonly JwtTokenService _jwtTokenService;

        // =====================================================
        // CONSTRUCTOR
        // =====================================================

        public IdentityService(
            UserManager<UsuarioSistema> userManager,
            RoleManager<IdentityRole> roleManager,
            JwtTokenService jwtTokenService,
            SignInManager<UsuarioSistema> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
            _signInManager = signInManager;
        }

        // =====================================================
        // INICIAR SESIÓN
        // =====================================================

        public async Task<LoginResponseDto?> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false
            );

            if (!result.Succeeded)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            var permissions = await GetPermissionsAsync(roles);
            var tokenResult = _jwtTokenService.GenerateToken(user, roles);

            return new LoginResponseDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Nombre = user.FirstName,
                    Apellido = user.LastName,
                    Roles = roles.ToList(),
                    Permissions = permissions
                },
                Token = tokenResult.Token,
                ExpiresAt = tokenResult.ExpiresAt
            };
        }

        // =====================================================
        // BUSCAR USUARIO POR EMAIL
        // =====================================================

        public async Task<UserDto> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return null;

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

        // =====================================================
        // REGISTRAR VISITANTE
        // =====================================================

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

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new DomainException(
                    string.Join(",", result.Errors.Select(x => x.Description))
                );
            }

            await _userManager.AddToRoleAsync(user, "Visitante");

            return user.Id;
        }

        // =====================================================
        // REGISTRAR PERSONAL DEL MUSEO
        // =====================================================

        public async Task<string> RegistrarPersonalAsync(
            string nombre,
            string apellido,
            string email,
            string telefono,
            DateOnly fechaNac,
            string rolUsuarioId,
            string password)
        {
            var user = new UsuarioSistema(nombre, apellido, fechaNac)
            {
                Email = email,
                UserName = email,
                PhoneNumber = telefono,
                BirthDate = fechaNac,
                TipoCuenta = TipoCuenta.PersonalMuseo
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new DomainException(
                    string.Join(",", result.Errors.Select(x => x.Description))
                );
            }

            var role = await _roleManager.FindByIdAsync(rolUsuarioId);

            if (role is null)
                throw new DomainException("El rol no existe.");

            await _userManager.AddToRoleAsync(user, role.Name);

            return user.Id;
        }

        // =====================================================
        // BUSCAR ROL POR ID
        // =====================================================

        public async Task<IdentityRole?> FindRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        // =====================================================
        // VERIFICAR SI EXISTE UN USUARIO POR EMAIL
        // =====================================================

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user is not null;
        }

        // =====================================================
        // BUSCAR USUARIO POR ID
        // =====================================================

        public async Task<UserDto?> FindById(string idUser)
        {
            var user = await _userManager.FindByIdAsync(idUser);

            if (user is null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            var permissions = await GetPermissionsAsync(roles);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Telefono = user.PhoneNumber,
                Nombre = user.FirstName,
                Apellido = user.LastName,
                Roles = roles.ToList(),
                Permissions = permissions
            };
        }


        // =====================================================
        // BUSCAR EMAIL POR ID DE USUARIO
        // =====================================================

        public async Task<string> FindEmailById(string idUser)
        {
            var user = await _userManager.FindByIdAsync(idUser);

            if (user is null)
                return null;

            return user.Email;
        }

        // =====================================================
        // GENERAR TOKEN DE CONFIRMACIÓN DE EMAIL
        // =====================================================

        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user is null
                ? throw new DomainException("El usuario no existe.")
                : await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        // =====================================================
        // CONFIRMAR EMAIL
        // =====================================================

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded;
        }

        // =====================================================
        // OBTENER TODOS LOS ROLES
        // =====================================================

        public async Task<RolDto[]> GetAllRolesAsync()
        {
            return await _roleManager.Roles
                .Select(r => new RolDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    NormalizedName = r.NormalizedName
                })
                .ToArrayAsync();
        }

        // =====================================================
        // BUSCAR USUARIOS POR LISTA DE IDS
        // =====================================================

        public async Task<List<UserDto>> FindByIdsAsync(List<string> ids)
        {
            return await _userManager.Users
                .Where(u => ids.Contains(u.Id))
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Nombre = u.FirstName,
                    Apellido = u.LastName
                })
                .ToListAsync();
        }

        // =====================================================
        // OBTENER TODO EL PERSONAL DEL MUSEO
        // =====================================================

        public async Task<List<UserDto>> GetPersonalMuseoAsync()
        {
            var usuarios = await _userManager.Users
                .Where(u => u.TipoCuenta == TipoCuenta.PersonalMuseo)
                .ToListAsync();

            var usuariosDto = new List<UserDto>();

            foreach (var user in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);

                usuariosDto.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Telefono = user.PhoneNumber,
                    Nombre = user.FirstName,
                    Apellido = user.LastName,
                    Roles = roles.ToList()
                });
            }

            return usuariosDto;
        }

        // =====================================================
        // ACTUALIZAR DATOS Y ROL DEL PERSONAL DEL MUSEO
        // =====================================================

        public async Task<bool> UpdatePersonalMuseoAsync(
            string userId,
            string nombre,
            string apellido,
            string telefono,
            string rolUsuarioId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.TipoCuenta != TipoCuenta.PersonalMuseo)
                throw new DomainException("El usuario no existe o no pertenece al personal del museo.");

            var role = await _roleManager.FindByIdAsync(rolUsuarioId);

            if (role is null)
                throw new DomainException("El rol no existe.");

            user.FirstName = nombre;
            user.LastName = apellido;
            user.PhoneNumber = telefono;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new DomainException(
                    string.Join(",", updateResult.Errors.Select(x => x.Description))
                );
            }

            var rolesActuales = await _userManager.GetRolesAsync(user);

            if (rolesActuales.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesActuales);

                if (!removeResult.Succeeded)
                {
                    throw new DomainException(
                        string.Join(",", removeResult.Errors.Select(x => x.Description))
                    );
                }
            }

            var addResult = await _userManager.AddToRoleAsync(user, role.Name!);

            if (!addResult.Succeeded)
            {
                throw new DomainException(
                    string.Join(",", addResult.Errors.Select(x => x.Description))
                );
            }

            return true;
        }

        // =====================================================
        // CAMBIAR CONTRASEÑA DEL PERSONAL (ADMINISTRADOR)
        // =====================================================

        public async Task<bool> ChangePasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.TipoCuenta != TipoCuenta.PersonalMuseo)
                throw new DomainException("El usuario no existe o no pertenece al personal del museo.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                throw new DomainException(
                    string.Join(",", result.Errors.Select(x => x.Description))
                );
            }

            return true;
        }

        // =====================================================
        // GENERAR TOKEN PARA RESTABLECER CONTRASEÑA
        // =====================================================

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                throw new DomainException("El usuario no existe.");

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        // =====================================================
        // RESTABLECER CONTRASEÑA CON TOKEN
        // =====================================================

        public async Task<bool> ResetPasswordAsync(
            string email,
            string token,
            string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                throw new DomainException("El usuario no existe.");

            var result = await _userManager.ResetPasswordAsync(
                user,
                token,
                newPassword
            );

            if (!result.Succeeded)
            {
                throw new DomainException(
                    string.Join(",", result.Errors.Select(x => x.Description))
                );
            }

            return true;
        }

        public async Task<string> GenerateChangeEmailTokenAsync(
            string userId,
            string nuevoEmail)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                throw new DomainException("El usuario no existe.");

            return await _userManager.GenerateChangeEmailTokenAsync(
                user,
                nuevoEmail
            );
        }
        public async Task<bool> ChangeEmailAsync(
            string userId,
            string nuevoEmail,
            string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                throw new DomainException("El usuario no existe.");

            var result = await _userManager.ChangeEmailAsync(
                user,
                nuevoEmail,
                token
            );

            if (!result.Succeeded)
            {
                throw new DomainException(
                    string.Join(",", result.Errors.Select(x => x.Description))
                );
            }

            return true;
        }

        private async Task<List<string>> GetPermissionsAsync(IList<string> roles)
        {
            var permissions = new HashSet<string>();

            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role is null)
                    continue;

                var claims = await _roleManager.GetClaimsAsync(role);

                foreach (var claim in claims)
                {
                    if (claim.Type == Permissions.ClaimType)
                    {
                        permissions.Add(claim.Value);
                    }
                }
            }

            return permissions.ToList();
        }
        public async Task<bool> UpdateUserNameAsync(
                string userId,
                string nuevoUserName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                throw new DomainException("El usuario no existe.");

            var result = await _userManager.SetUserNameAsync(
                user,
                nuevoUserName
            );

            if (!result.Succeeded)
            {
                throw new DomainException(
                    string.Join(",", result.Errors.Select(x => x.Description))
                );
            }

            return true;
        }
    }
}