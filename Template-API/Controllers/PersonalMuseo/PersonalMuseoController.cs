using Application.PersonalMuseo.UseCases.Commands;
using Application.PersonalMuseo.UseCases.Commands.CambioConstraseña;
using Application.PersonalMuseo.UseCases.Queries.GetById;
using Application.Usuario.UseCases.Commands.UpdateUsuario;
using Application.Usuario.UseCases.Queries.GetAllUser;
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
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var result = await _commandQueryBus.Send(request);

            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllUserQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }

        [HttpPut("{userId}/update")]
        public async Task<IActionResult> UpdatePersonalMuseo(
            string userId,
            [FromBody] UpdatePersonalMuseoCommand command)
        {
            command.UserId = userId;

            await _commandQueryBus.Send(command);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var entity = await _commandQueryBus.Send(new GetUserByQuery { UserId = id });

            return Ok(entity);
        }

        [HttpPost("{userId}/solicitar-cambio-email")]
        public async Task<IActionResult> SolicitarCambioEmail(
            string userId,
            [FromBody] SolicitarCambioEmailCommand command)
        {
            command.UserId = userId;

            var result = await _commandQueryBus.Send(command);

            return Ok(result);
        }

        [HttpPost("confirmar-cambio-email")]
        public async Task<IActionResult> ConfirmarCambioEmail(
            [FromBody] ConfirmarCambioEmailCommand command)
        {
            var result = await _commandQueryBus.Send(command);

            return Ok(result);
        }


    }
}