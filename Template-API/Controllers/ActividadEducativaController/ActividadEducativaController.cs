using Application.ActividadesEducativas.UseCases.Commands.CancelarActEducativa;
using Application.ActividadesEducativas.UseCases.Commands.CrearActividadEducativa;
using Application.ActividadesEducativas.UseCases.Commands.UpdateActividaEducativa;
using Application.ActividadesEducativas.UseCases.Queries;
using Application.ActividadesEducativas.UseCases.Queries.GetActEducacion;
using Application.ActividadesEducativas.UseCases.Queries.GetActividadesEducativas;
using Application.ActividadesEducativas.UseCases.Queries.GetById;
using Application.ActividadesEducativas.UseCases.Queries.ProyectosEducativos;
using Application.ActividadesEducativas.UseCases.Queries.SalasDispActEducativas;
using Application.Eventos.DataTransferObjets;

using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.ActividadEducativaController
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ActividadEducativaController(ICommandQueryBus commandQueryBus, IWebHostEnvironment environment,
        IConfiguration configuration) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus;
        private readonly IWebHostEnvironment _environment = environment;
        private readonly IConfiguration _configuration = configuration;

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(
            [FromForm] CrearActividadEducativaCommand command,
            [FromForm] List<IFormFile>? imagenes)
        {
            if (command is null)
                return BadRequest();

            var urls = command.UrlImagenes?.ToList() ?? [];

            if (imagenes is not null && imagenes.Count > 0)
            {
                var uploadsDirectory = Path.Combine(
                    _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot"),
                    "uploads",
                    "actividadeducativa");

                Directory.CreateDirectory(uploadsDirectory);

                foreach (var imagen in imagenes.Where(i => i.Length > 0))
                {
                    var extension = Path.GetExtension(imagen.FileName);
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsDirectory, fileName);

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await imagen.CopyToAsync(stream);

                    urls.Add(BuildPublicImageUrl(fileName));
                }
            }

            command.UrlImagenes = urls;

            var actividadId = await _commandQueryBus.Send(command);
            return Created($"api/[Controller]/{actividadId}", new { Id = actividadId });
        }
        [HttpPut("update-actividad-educativa")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateActividadEducativa(
            [FromForm] UpdateActividadEducativaCommand command,
            [FromForm] List<IFormFile>? imagenes)
        {
            if (command is null)
                return BadRequest();

            command.ImagenesNuevas = [];

            if (imagenes is not null && imagenes.Count > 0)
            {
                foreach (var img in imagenes.Where(i => i.Length > 0))
                {
                    using var memoryStream = new MemoryStream();
                    await img.CopyToAsync(memoryStream);

                    command.ImagenesNuevas.Add(new ArchivoNuevoDto
                    {
                        Nombre = img.FileName,
                        ContentType = img.ContentType,
                        Contenido = memoryStream.ToArray()
                    });
                }
            }

            var actividadActualizada = await _commandQueryBus.Send(command);

            if (actividadActualizada is null)
                return NotFound(
                    $"No se encontró la actividad educativa con el ID {command.ActividadEducativaId}.");

            return Ok(actividadActualizada);
        }
        [HttpGet("actividadeseducativas")]
        public async Task<IActionResult> GetActividadesEducativas(
        [FromQuery] DateTime desde,
        [FromQuery] DateTime hasta)
        {
            if (desde == default || hasta == default)
            {
                return BadRequest("Parámetros 'desde' y 'hasta' requeridos en la query. Formato ISO: yyyy-MM-dd o yyyy-MM-ddTHH:mm:ss");
            }

            var actEducativas = await _commandQueryBus.Send(new ActividadesEducativasQuery(desde, hasta));

            return Ok(actEducativas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var entity = await _commandQueryBus.Send(new GetActividadEducativaQuery { ActividadId = id });

            return Ok(entity);
        }

        [HttpGet("actividadeseducacion")]
        public async Task<IActionResult> GetReporteActEducacion(
           [FromQuery] DateTime desde,
           [FromQuery] DateTime hasta)
        {

            if (desde == default || hasta == default)
            {
                return BadRequest("Parámetros 'desde' y 'hasta' requeridos en la query. Formato ISO: yyyy-MM-dd o yyyy-MM-ddTHH:mm:ss");
            }
            if (desde > hasta)
            {
                return BadRequest(
                    "El parámetro 'desde' no puede ser posterior a 'hasta'."
                );
            }

            var actividades = await _commandQueryBus.Send(new ActividadesEducacionQuery(desde, hasta));

            return Ok(actividades);
        }

        [HttpGet("disponibilidadactividadeseducativas")]
        public async Task<IActionResult> ObtenerDisponibilidadEvento(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta,
            [FromQuery] List<string> salasIds,
            [FromQuery] string? actividadEducativaIdExcluir = null)
        {
            var query = new DispActividadesEducativasQuery(
                desde,
                hasta,
                salasIds,
                actividadEducativaIdExcluir);

            var resultado = await _commandQueryBus.Send(query);

            return Ok(resultado);
        }
        [HttpGet("proyectos-educativos")]
        public async Task<IActionResult> ObtenerProyectosEducativosQuery()
        {
            var query = new ObtenerProyectosEducativosQuery();
            var resultado = await _commandQueryBus.Send(query);
            return Ok(resultado);
        }
        [HttpGet("salas-disponibles-actEducativas")]
        public async Task<IActionResult> ObtenerSalasDisponiblesActEduc()
        {
            var query = new ObtenerSalasParaActEducQuery();
            var resultado = await _commandQueryBus.Send(query);
            return Ok(resultado);
        }
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            await _commandQueryBus.Send(
                new CancelarActEducativaCommand
                {
                    ActEducativaId = id
                });

            return NoContent();
        }
        private string BuildPublicImageUrl(string fileName)
        {
            var configuredBaseUrl = _configuration["Images:PublicBaseUrl"];
            var baseUrl = string.IsNullOrWhiteSpace(configuredBaseUrl)
                ? string.Empty
                : configuredBaseUrl.TrimEnd('/');

            if (!string.IsNullOrEmpty(baseUrl))
                return $"{baseUrl}/uploads/actividadeducativa/{fileName}";

            return $"/uploads/actividadeducativa/{fileName}";
        }


    }
}

