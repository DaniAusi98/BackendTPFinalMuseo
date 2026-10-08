using Application.PersonalMuseo.UseCases.Commands;
using Application.PersonalMuseo.UseCases.Queries.ObtenerAreas;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.PersonalMuseo
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonalMuselController(ICommandQueryBus commandQueryBus) : BaseController
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
            var areas = await _commandQueryBus.Send(new ObtenerPuestoAreaQuery());
            return Ok(areas);
        }
    }
}