using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.UseCases.Commands.CreateConfiguracionVisitasGrupalesGuiadas;
using Application.VisitaGrupal.UseCases.Commands.UpdateConfiguracionVisitasGrupalesGuiadas;
using Application.VisitaGrupal.UseCases.Commands.DeleteConfiguracionVisitasGrupalesGuiadas;
using Application.VisitaGrupal.UseCases.Queries.GetConfiguracionVisitasGrupalesGuiadas;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.VisitasGrupales
{
    [ApiController]
    public class ConfiguracionVisitasGrupalesGuiadasController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));

        [HttpGet("api/v1/[Controller]")]
        public async Task<IActionResult> Get()
        {
            var result = await _commandQueryBus.Send(new GetConfiguracionVisitasGrupalesGuiadasQuery());
            return Ok(result);
        }

        [HttpPost("api/v1/[Controller]")]
        public async Task<IActionResult> Create(CreateConfiguracionVisitasGrupalesGuiadasCommand command)
        {
            if (command is null) return BadRequest();
            var id = await _commandQueryBus.Send(command);
            return Created($"api/[Controller]/{id}", new { Id = id });
        }

        [HttpPut("api/v1/[Controller]")]
        public async Task<IActionResult> Update(UpdateConfiguracionVisitasGrupalesGuiadasCommand command)
        {
            if (command is null) return BadRequest();
            await _commandQueryBus.Send(command);
            return NoContent();
        }

        [HttpDelete("api/v1/[Controller]")]
        public async Task<IActionResult> Delete()
        {
            await _commandQueryBus.Send(new DeleteConfiguracionVisitasGrupalesGuiadasCommand());
            return NoContent();
        }
    }
}
