using Application.VisitaGrupal.UseCases.Commands.CreateSettingSelfGuidedTours;
using Application.VisitaGrupal.UseCases.Queries.GetConfiguracionVisitasAutoguiadas;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.VisitasGrupales
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ConfigVisitasAutoguiadasController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));

        [HttpGet("get-settings-self-guided-tours")]
        public async Task<IActionResult> Get()
        {
            var result = await _commandQueryBus.Send(new GetSettingSelfGuidedToursQuery());
            return Ok(result);
        }

        [HttpPost("create-settings-self-guided-tours")]
        public async Task<IActionResult> Create(CreateSettingSelfGuidedToursCommand command)
        {
            if (command is null) return BadRequest();
            var id = await _commandQueryBus.Send(command);
            return Created($"api/[Controller]/{id}", new { Id = id });
        }
    }
}
