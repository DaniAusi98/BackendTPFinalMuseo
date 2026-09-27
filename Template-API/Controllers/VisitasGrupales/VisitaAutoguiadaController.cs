using Application.VisitaGrupal.UseCases.Comands.ConfirmarVisitaGrupal;
using Application.VisitaGrupal.UseCases.Comands.CrearVisitaAutoguiada;
using Application.VisitaGrupal.UseCases.Comands.CrearVisitaGuiada;
using Application.VisitaGrupal.UseCases.Comands.NewFolder;
using Application.VisitaGrupal.UseCases.Queries.GetReservationById;
using Application.VisitaGrupal.UseCases.Queries.GetReservationsByUserId;
using Application.VisitaGrupal.UseCases.Queries.ReporteVisitaAutoguiada;
using Core.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Controllers.VisitasGrupales
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VisitaAutoguiadaController(ICommandQueryBus commandQueryBus) : ControllerBase
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus;

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Create(
        CrearVisitaAutoguiadaCommand command)
        {
            if (command is null)
                return BadRequest();
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            command.UsuarioVisitanteId = usuarioId;
            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });

        }

        [HttpGet("DisponibilidadTurnosVisitasAutoguiadas")]
        public async Task<IActionResult> DisponibilidadTurnosVisitasAutoguiadas(
          [FromQuery] DateTime fechaDesde,
          [FromQuery] DateTime fechaHasta,
          [FromQuery] uint pageIndex = 1,
          [FromQuery] uint pageSize = 10)
        {
            var visitas = await _commandQueryBus.Send(
                new Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaAutoguiada.DisponibilidadTurnosVisitasAutoguiadasQuery(fechaDesde, fechaHasta)
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize
                });

            return Ok(visitas);
        }

        [HttpPatch("{id}/confirmar")]
        public async Task<IActionResult> Confirmar(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            await _commandQueryBus.Send(
                new ConfirmarAutoguiadaCommand
                {
                    ReservationId = id
                });
            return NoContent();
        }
        [HttpGet("reporteautoguiadas")]
    public async Task<IActionResult> GetReporteVisitaAutoGuiada(
        [FromQuery] DateTime desde,
        [FromQuery] DateTime hasta)
    {
        if (desde == default || hasta == default)
        {
            return BadRequest("Parámetros 'desde' y 'hasta' requeridos en la query. Formato ISO: yyyy-MM-dd o yyyy-MM-ddTHH:mm:ss");
        }

        var reporte = await _commandQueryBus.Send(new GetReportAllSelfGuidedToursQuery(desde, hasta));
      
        return Ok(reporte);
    }

        //GET /api/Visitas/DisponibilidadTurnosVisitasAutoguiadas?fechaDesde=2026-08-10&fechaHasta=2026-08-15&salaIds=1&salaIds=3&salaIds=5&pageIndex=1&pageSize=10

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{id}/reprogramar")]
        public async Task<IActionResult> Reprogram(
       string id,
       ReprogramarCommand command)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            command.UsuarioVisitanteId = usuarioId;
            command.VisitaReprogramadaId = id;

            var nuevaVisitaId = await _commandQueryBus.Send(command);

            return Created($"api/v1/VisitasAutoguiadas/{nuevaVisitaId}", new { Id = nuevaVisitaId });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("reservations")]
        public async Task<IActionResult> GetReservations()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId)) return Unauthorized();

            var visitas = await _commandQueryBus.Send(
                new GetSelfGuidedByUserIdQuery
                {
                    UsuarioVisitanteId = usuarioId
                });

            return Ok(visitas);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            var entity = await _commandQueryBus.Send(new GetSelfGuidedByIdQuery { VisitaId = id });
            return Ok(entity);
        }
    }

}
