using Core.Application;
using Application.MuseumResources.UseCases.Recurso.Commands.CreateRecurso;
using Application.MuseumResources.UseCases.Recurso.Queries.GetAllRecursos;
using Application.MuseumResources.UseCases.Recurso.Queries.GetRecursoBy;
using Application.MuseumResources.UseCases.Recurso.Commands.UpdateRecurso;
using Application.MuseumResources.UseCases.Recurso.Commands.DeleteRecurso;
using Microsoft.AspNetCore.Mvc;
using Application.MuseumResources.UseCases.Recurso.Queries.DisponibilidadRecursos;

namespace Controllers
{
    [ApiController]
    public class RecursoController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));

        [HttpGet("api/v1/[Controller]")]
        public async Task<IActionResult> GetAll(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllRecursosQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }

        [HttpGet("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id<0 || id>int.MaxValue) return BadRequest();

            var entity = await _commandQueryBus.Send(new GetRecursoByQuery { RecursoId = id });

            return Ok(entity);
        }
        [HttpGet("api/v1/[Controller]/disponibilidad")]
        public async Task<IActionResult> GetDisponibilidad(
           [FromQuery] DateTime inicio,
           [FromQuery] DateTime fin,
           [FromQuery] string? rrule)
        {
            if (fin <= inicio)
                return BadRequest("La fecha de fin debe ser posterior a la fecha de inicio.");

            var resultado = await _commandQueryBus.Send(
                new GetDisponibilidadRecursosPorActividadQuery
                {
                    Inicio = inicio,
                    Fin = fin,
                    RRule = rrule
                });

            return Ok(resultado);
        }

        [HttpPost("api/v1/[Controller]")]
        public async Task<IActionResult> Create(CreateRecursoCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });
        }

        [HttpPut("api/v1/[Controller]")]
        public async Task<IActionResult> Update(UpdateRecursoCommand command)
        {
            if (command is null) return BadRequest();

            await _commandQueryBus.Send(command);

            return NoContent();
        }

        [HttpDelete("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0) return BadRequest();

            await _commandQueryBus.Send(new DeleteRecursoCommand { RecursoId = id });

            return NoContent();
        }
    }
}
