using Application.PersonalMuseo.UseCases.Commands;
using Application.PersonalMuseo.UseCases.Commands.GetUserMuseoById;
using Application.PersonalMuseo.UseCases.Commands.LoginPersonal;
using Application.PersonalMuseo.UseCases.Queries.ObtenerAreas;
using Application.PersonalMuseo.UseCases.Queries.ObtenerPuestosArea;
using Core.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Controllers.PersonalMuseo
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonalMuseoController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus =
            commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));

        [HttpPost("RegistrarPersonal")]
        public async Task<IActionResult> Create(RegistrarPersonalQuery command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });
        }
        [HttpGet("GetAllAreas")]
        public async Task<IActionResult> GetAllAreas()
        {
            var areas = await _commandQueryBus.Send(new ObtenerAreasQuery());
            return Ok(areas);
        }

        [HttpGet("puestos-area")]
        public async Task<IActionResult> GetPuestosArea([FromQuery] string areaId)
        {
            var puestos = await _commandQueryBus.Send(new ObtenerPuestoAreaQuery(areaId));
            return Ok(puestos);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("get-museo-user")]
        public async Task<IActionResult> GetMyUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _commandQueryBus.Send(
                new GetUserMuseoByIdQuery
                {
                    UserMuseoId = userId
                });

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginPersonalCommand command)
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
    }
}