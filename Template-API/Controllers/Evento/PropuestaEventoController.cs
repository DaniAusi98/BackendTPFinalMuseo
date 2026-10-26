using Application.Eventos.UseCases.Commands.CrearPropuesta;
using Application.Eventos.UseCases.Commands.ReenviarFormularioEvento;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Evento
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PropuestaEventoController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus;

        [HttpPost("crear-propuesta")]
        public async Task<IActionResult> Create(CrearPropuestaCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });
        }


        [HttpPost("{id}/reenviar-formulario")]
        public async Task<IActionResult> ReenviarFormulario(
            string id,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            await _commandQueryBus.Send(new ReenviarFormularioEventoCommand { IdEvento = id });

            return Ok(new
            {
                Message = "El formulario fue enviado nuevamente."
            });
        }
    }
}
