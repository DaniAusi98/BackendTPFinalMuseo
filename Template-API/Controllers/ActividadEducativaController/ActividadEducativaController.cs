using Application.ActividadesEducativas.UseCases.Commands.CrearActividadEducativa;
using Application.ActividadesEducativas.UseCases.Queries;
using Application.ActividadesEducativas.UseCases.Queries.ProyectosEducativos;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.ActividadEducativaController
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ActividadEducativaController(ICommandQueryBus commandQueryBus) : BaseController
    {

        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus;
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CrearActividadEducativa(
            CrearActividadEducativaCommand command)
        {
            if (command is null)
                return BadRequest();

            var actividadId = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{actividadId}", new { Id = actividadId });


        }
        [HttpGet("disponibilidadactividadeseducativas")]
        public async Task<IActionResult> ObtenerDisponibilidadEvento(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta,
            [FromQuery] List<string> salasIds)
        {
            var query = new DispActividadesEducativasQuery(
                desde,
                hasta,
                salasIds);

            var resultado = await _commandQueryBus.Send(query);

            return Ok(resultado);
        }
        [HttpGet("proyectos-educativos")]
        public async Task<IActionResult> ObtenerSalasDisponiblesParaEvento()
        {
            var query = new ObtenerProyectosEducativosQuery();
            var resultado = await _commandQueryBus.Send(query);
            return Ok(resultado);
        }



    }
}

