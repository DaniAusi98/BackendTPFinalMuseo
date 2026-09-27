using Application.Eventos.UseCases.Queries;
using Application.MuseumResources.UseCases.MuseumGallery.Commands.CreateMuseumGallery;
using Application.MuseumResources.UseCases.MuseumGallery.Commands.DeleteMuseumGallery;
using Application.MuseumResources.UseCases.MuseumGallery.Commands.UpdateMuseumGallery;
using Application.MuseumResources.UseCases.MuseumGallery.Queries.GetAllSalas;
using Application.MuseumResources.UseCases.MuseumGallery.Queries.GetSalaBy;
using Application.MuseumResources.UseCases.MuseumGallery.Queries.SalasDisponibles;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    public class MuseumGalleryController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));

        [HttpGet("api/v1/[Controller]")]
        public async Task<IActionResult> GetAll(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllMuseumGalleriesQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }

        [HttpGet("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id<0 || id>int.MaxValue) return BadRequest();

            var entity = await _commandQueryBus.Send(new GetMuseumGalleryByQuery { SalaId = id });

            return Ok(entity);
        }

        [HttpPost("api/v1/[Controller]")]
        public async Task<IActionResult> Create(CreateMuseumGalleryCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });
        }

        [HttpPut("api/v1/[Controller]")]
        public async Task<IActionResult> Update(UpdateMuseumGalleryCommand command)
        {
            if (command is null) return BadRequest();

            await _commandQueryBus.Send(command);

            return NoContent();
        }

        [HttpDelete("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0) return BadRequest();

            await _commandQueryBus.Send(new DeleteMuseumGalleryCommand { MuseumGalleryId = id });

            return NoContent();
        }

        [HttpGet("salas-disponibles")]
        public async Task<IActionResult> ObtenerSalasDisponiblesParaEvento()
        {
            var query = new ObtenerSalasParaEventoQuery();
            var resultado = await _commandQueryBus.Send(query);
            return Ok(resultado);
        }
    }
}
