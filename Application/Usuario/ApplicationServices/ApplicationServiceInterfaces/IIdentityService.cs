

using Application.Usuario.DataTransferObjets;
using Microsoft.AspNetCore.Identity;

namespace Application.Usuario.ApplicationServices.ApplicationServiceInterfaces
{
    public interface IIdentityService
    {

        Task<string> RegisterAsync(string nombre, string apellido, DateOnly fechaNac, string email, string telefono, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<string> RegistrarPersonalAsync(
        string nombre,
        string apellido,
        string email,
        string telefono,
        DateOnly fechaNac,
        string rolUsuarioId,
        string password);
        Task<LoginResponseDto?> LoginAsync(string email, string password);
        Task<UserDto?> FindByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
        Task<UserDto?> FindById(string idUser);
        Task<string> FindEmailById(string idUser);
        Task<RolDto[]> GetAllRolesAsync();
        Task<List<UserDto>> FindByIdsAsync(List<string> ids);
        Task<List<UserDto>> GetPersonalMuseoAsync();
        Task<bool> ChangePasswordAsync(string userId, string newPassword);
        Task<bool> UpdatePersonalMuseoAsync(
        string userId,
        string nombre,
        string apellido,
        string telefono,
        string rolUsuarioId);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
        Task<IdentityRole?> FindRoleByIdAsync(string roleId);
        Task<bool> ChangeEmailAsync(string userId, string nuevoEmail, string token);
        Task<bool> UpdateUserNameAsync(string userId, string nuevoUserName);
        Task<string> GenerateChangeEmailTokenAsync(string userId, string nuevoEmail);
    }
}
