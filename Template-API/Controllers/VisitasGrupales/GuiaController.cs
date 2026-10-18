using Application.VisitaGrupal.UseCases.Guias.Commands.CrearAusenciaGuia;
using Application.VisitaGrupal.UseCases.Guias.Commands.CrearNoDisponibilidadGuia;
using Application.VisitaGrupal.UseCases.Guias.Commands.CreateGuia;
using Application.VisitaGrupal.UseCases.Guias.Commands.DeleteGuia;
using Application.VisitaGrupal.UseCases.Guias.Commands.UpdateGuia;
using Application.VisitaGrupal.UseCases.Guias.Queries.DisponibilidadGuia;
using Application.VisitaGrupal.UseCases.Guias.Queries.DisponibilidadGuias;
using Application.VisitaGrupal.UseCases.Queries.GetAllGuias;
using Application.VisitaGrupal.UseCases.Queries.GetAusenciasGuias;
using Application.VisitaGrupal.UseCases.Queries.GetGuiaBy;
using Core.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Controllers.VisitasGrupales
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GuiaController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));

        [HttpGet("getGuidesActives")]
        public async Task<IActionResult> GetAll(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllGuiasActivesQuery());

            return Ok(entities);
        }
        [HttpGet("getGuides")]
        public async Task<IActionResult> GetAllGuidesActives(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllGuiasQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }
        [HttpGet("getabsences")]
        public async Task<IActionResult> ObtenerAusencias(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllAusenciasGuiasQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            var entity = await _commandQueryBus.Send(
                new GetGuiaByQuery { GuiaId = id }
            );

            return Ok(entity);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Create([FromBody] CreateGuiaCommand command)
        {
            if (command is null)
                return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { Id = id }
            );
        }
        [HttpPost("ausenciasGuias")]
        public async Task<IActionResult> CrearAusencia(CrearAusenciaCommand command)
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



        [HttpGet("DisponibilidadGuia")]
        public async Task<IActionResult> DisponibilidadGuia(
            [FromQuery] string guiaId,
            [FromQuery] DateTime fechaDesde,
            [FromQuery] DateTime fechaHasta
          )
        {
            var guias = await _commandQueryBus.Send(
                new DisponibilidadGuiaQuery(fechaDesde, fechaHasta, guiaId)
                {
                });

            return Ok(guias);
        }
        [HttpGet("DisponibilidadGuias")]
        public async Task<IActionResult> DisponibilidadGuias(
            [FromQuery] DateTime fechaDesde,
            [FromQuery] DateTime fechaHasta
          )
        {
            var guias = await _commandQueryBus.Send(
                new DisponibilidadGuiasCalendarQuery(fechaDesde, fechaHasta)
                {
                });

            return Ok(guias);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("DisponGuiaAutenticado")]
        public async Task<IActionResult> DisponGuiaAutenticado(
            [FromQuery] DateTime fechaDesde,
            [FromQuery] DateTime fechaHasta)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var disponGuia = await _commandQueryBus.Send(
                new DisponGuiaUserIdQuery(fechaDesde, fechaHasta, userId)
                {
                });

            return Ok(disponGuia);
        }







        [HttpPost("NoDisponibilidadGuia")]
        public async Task<IActionResult> CrearAusenciaProgramada(CrearNoDisponibilidadGuiaCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });
        }



    }
}
