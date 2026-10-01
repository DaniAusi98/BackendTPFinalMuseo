using Application.VisitaGrupal.UseCases.Guias.Commands.CreateGuia;
using Application.VisitaGrupal.UseCases.Guias.Commands.UpdateGuia;
using Application.VisitaGrupal.UseCases.Queries.GetAllGuias;
using Application.VisitaGrupal.UseCases.Queries.GetGuiaBy;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.VisitasGrupales
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GuiaController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));

        [HttpGet("api/v1/[Controller]")]
        public async Task<IActionResult> GetAll(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllGuiasQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }

        [HttpGet("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 0 || id > int.MaxValue) return BadRequest();

            var entity = await _commandQueryBus.Send(new GetGuiaByQuery { GuiaId = id });

            return Ok(entity);
        }

        [HttpPost("api/v1/[Controller]")]
        public async Task<IActionResult> Create(CreateGuiaCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });
        }

        [HttpPut("api/v1/[Controller]")]
        public async Task<IActionResult> Update(UpdateGuiaCommand command)
        {
            if (command is null) return BadRequest();

            await _commandQueryBus.Send(command);

            return NoContent();
        }

        [HttpDelete("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            await _commandQueryBus.Send(new DeleteGuiaCommand { GuiaId = id });

            return NoContent();
        }



        [HttpGet("DisponibilidadGuias")]
        public async Task<IActionResult> DisponibilidadGuias(
            [FromQuery] DateTime fechaDesde,
            [FromQuery] DateTime fechaHasta,
            [FromQuery] uint pageIndex = 1,
            [FromQuery] uint pageSize = 10)
        {
            var guias = await _commandQueryBus.Send(
                new Application.VisitaGrupal.UseCases.Guias.Queries.ConsultarDisponibilidadGuiasQuery(fechaDesde, fechaHasta)
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize
                });
            return Ok(guias);
        }


    }
}
