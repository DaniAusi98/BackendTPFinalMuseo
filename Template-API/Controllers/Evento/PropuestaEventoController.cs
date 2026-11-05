using Application.Eventos.UseCases.Commands.CrearPropuestaEvento;
using Application.Eventos.UseCases.Commands.ReenviarFormularioEvento;
using Application.Eventos.UseCases.Queries.GetAllPropuestas;
using Application.Eventos.UseCases.Queries.GetAllPropuestas.GetAllExhibitions;
using Application.Eventos.UseCases.Queries.GetPropuestaByQuery;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Evento
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PropuestaEventoController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus;



        [HttpPost("crear-propuesta-evento")]
        public async Task<IActionResult> Create(CrearPropuestaEventoCommand command)
        {
            if (command is null) return BadRequest();

            var eventoId = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{eventoId}", new { Id = eventoId });
        }



        [HttpPost("crear-propuesta-muestra")]
        public async Task<IActionResult> Create(PropuestaMuestrasTemporalesCommand command)
        {
            if (command is null) return BadRequest();

            var muestraid = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{muestraid}", new { Id = muestraid });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllPropuestasEventoQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }

        [HttpGet("get-all-exhibitions")]
        public async Task<IActionResult> GetAllExhibitions(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllProposalsExhibitionsQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }
        [HttpGet("get-by")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var entity = await _commandQueryBus.Send(new GetPropuestaByIdQuery { PropuestaId = id });

            return Ok(entity);
        }








        [HttpPost("{id}/reenviar-formulario")]
        public async Task<IActionResult> ReenviarFormulario(
           string id,
           CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            await _commandQueryBus.Send(new ReenviarFormularioEventoCommand { IdPropuesta = id });

            return Ok(new
            {
                Message = "El formulario fue enviado nuevamente."
            });
        }




    }
}
