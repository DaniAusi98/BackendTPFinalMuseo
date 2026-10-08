using Application.Usuario.UseCases.Commands.LoginUsuario;
using Application.Usuario.UseCases.Queries.GetUsuarioById;
using Core.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsuarioController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus =
            commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioCommand command)
        {
            if (command is null) return BadRequest();

            try
            {
                var result = await _commandQueryBus.Send(command);

                // Devuelve exactamente el JSON que espera el frontend
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _commandQueryBus.Send(
                new GetUserByIdQuery
                {
                    UsuarioId = userId
                });

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("getAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var entities = await _commandQueryBus.Send(new GetAllRolesQuery());

            return Ok(entities);
        }

    }
}
