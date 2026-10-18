using Application.PersonalMuseo.UseCases.Commands;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

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


    }
}