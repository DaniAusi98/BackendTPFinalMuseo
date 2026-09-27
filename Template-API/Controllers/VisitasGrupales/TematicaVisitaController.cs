using Core.Application;

using Microsoft.AspNetCore.Mvc;
using Application.VisitaGrupal.UseCases.Tematicas.Commands.CreateTematica;
using Application.VisitaGrupal.UseCases.Tematicas.Commands.UpdateTematica;
using Application.VisitaGrupal.UseCases.Queries.GetTematicaVisitaGrupal;
using Application.VisitaGrupal.UseCases.Tematicas.Queries.GetTematicaBy;
using Application.VisitaGrupal.UseCases.Tematicas.Commands.DeleteTematica;

namespace Controllers.VisitasGrupales
{
    [ApiController]
    public class TematicaVisitaController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));

        [HttpGet("api/v1/[Controller]")]
        public async Task<IActionResult> GetAll(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllTematicasQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }

        [HttpGet("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id<0 || id>int.MaxValue) return BadRequest();

            var entity = await _commandQueryBus.Send(new GetTematicaByQuery { TematicaId = id });

            return Ok(entity);
        }

        [HttpPost("api/v1/[Controller]")]
        public async Task<IActionResult> Create(CreateTematicaCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });
        }

        [HttpPut("api/v1/[Controller]")]
        public async Task<IActionResult> Update(UpdateTematicaCommand command)
        {
            if (command is null) return BadRequest();

            await _commandQueryBus.Send(command);

            return NoContent();
        }

        [HttpDelete("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0) return BadRequest();

            await _commandQueryBus.Send(new DeleteTematicaCommand { TematicaId = id });

            return NoContent();
        }
    }
}
