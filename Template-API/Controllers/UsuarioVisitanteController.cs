using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.UseCases.Commands.LoginUsuario;
using Application.Usuario.UseCases.Commands.Register;
using Application.Usuario.UseCases.Commands.UpdateUsuario;
using Application.Usuario.UseCases.Queries;

using Core.Application;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsuarioVisitanteController(ICommandQueryBus commandQueryBus,IConfirmEmailService confirmarEmailService) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus =
            commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));
        private readonly IConfirmEmailService IConfirmarEmailService =
            confirmarEmailService ?? throw new ArgumentNullException(nameof(confirmarEmailService));


        /// <summary>
        /// Confirma el correo de un usuario a partir de un token recibido por email
        /// </summary>
        [HttpGet("confirmar-email")]
        public async Task<IActionResult> ConfirmarEmail(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest("Token inválido.");

            // Enviamos el token al handler de dominio que valida y confirma
            _ = await _commandQueryBus.Send(new ConfirmarEmailUsuarioCommand { Token = token });

            return Ok("Correo confirmado correctamente.");
        }

        /// <summary>
        /// Crea un nuevo usuario visitante
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Create( [FromBody] RegistrarVisitanteCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/v1/[Controller]/{id}", new { Id = id });
        }

        /// <summary>
        /// Login de usuario visitante: devuelve token + datos de usuario
        /// </summary>
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


        /*[HttpGet("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest();

            var entity = await _commandQueryBus.Send(new GetDummyEntityByQuery { DummyIdProperty = id });

            return Ok(entity);
        }

        */

        /* [HttpGet("api/v1/[Controller]/{id}")]
         public async Task<IActionResult> GetById(int id)
         {
             if (id <= 0) return BadRequest();

             var entity = await _commandQueryBus.Send(new GetDummyEntityByQuery { DummyIdProperty = id });

             return Ok(entity);
         }
        */

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _commandQueryBus.Send(
                new GetUsuarioVisitanteByIdQueryCommand
                {
                    UsuarioVisitanteId = userId
                });

            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(
            [FromBody] ConfirmEmailRequest request)
        {
            var result = await IConfirmarEmailService.ExecuteAsync(
                request.UserId,
                request.Token);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Error al confirmar el correo electrónico. El token puede ser inválido o haber expirado."
                });
            }

            return Ok(new
            {
                message = "Correo confirmado correctamente."
            });
        }

    }
}
