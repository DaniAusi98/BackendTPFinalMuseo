using Application.Eventos.DataTransferObjets;
using Application.Eventos.UseCases.Commands.CrearEvento;
using Application.Eventos.UseCases.Commands.UpdateEvento;
using Application.Eventos.UseCases.Queries;
using Application.Eventos.UseCases.Queries.GetEventoBy;
using Application.Eventos.UseCases.Queries.ReporteEventos;
using Application.Eventos.UseCases.Queries.SalasDisponibles;
using Application.MuseumResources.UseCases.Commands.ConfigurarSalaActividades;

using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Evento
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EventosController(
        ICommandQueryBus commandQueryBus,
        IWebHostEnvironment environment,
        IConfiguration configuration) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus;
        private readonly IWebHostEnvironment _environment = environment;
        private readonly IConfiguration _configuration = configuration;

        // ============================================================
        // FORMULARIO DIRECTO CREAR EVENTO O ACTIVIDAD EXTERNA 
        // ============================================================
        /// <summary>
        /// Es el que utiliza administracion para crear eventos que no necesitan presentar propuesta a direccion. 
        /// </summary>

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(
            [FromForm] CrearEventoCommand command,
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
                    "eventos");

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

            var id = await _commandQueryBus.Send(command);
            return Created($"api/v1/eventos/{id}", new { Id = id });
        }


        // ============================================================
        // GET  EVENTO BY ID
        // ============================================================


        [HttpGet("get-by")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var entity = await _commandQueryBus.Send(new GetEventoByIdQuery { EventoId = id });

            return Ok(entity);
        }

        // ============================================================
        // ACTUALIZAR EVENTO
        // ============================================================
        [HttpPut("update-evento")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(
        [FromForm] UpdateEventoCommand command,
        [FromForm] List<IFormFile>? imagenes)
        {
            if (command is null) return BadRequest();

            // Mapeamos los archivos binarios HTTP nativos al DTO puro de bytes
            if (imagenes is not null && imagenes.Count > 0)
            {
                command.ImagenesNuevas = [];

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

            // Enviamos el comando limpio a la capa de aplicación
            var eventoActualizado = await _commandQueryBus.Send(command);

            if (eventoActualizado == null)
            {
                return NotFound($"No se encontró el registro con el ID {command.EventoId}.");
            }

            // Retornamos el objeto modificado con un 200 OK
            return Ok(eventoActualizado);
        }


        ///Disponibilidad para el formCalendar///


        [HttpGet("disponibilidad")]
        public async Task<IActionResult> ObtenerDisponibilidadEvento(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta,
            [FromQuery] List<string> salasIds)
        {
            var query = new ObtenerDisponibilidadEventoQuery(
                desde,
                hasta,
                salasIds);

            var resultado = await _commandQueryBus.Send(query);

            return Ok(resultado);
        }

        ///Validacion de recurrencia Form///

        [HttpGet("validar-recurrencia")]
        public async Task<IActionResult> ValidarRecurrenciaEvento(
            [FromQuery] DateTime inicio,
            [FromQuery] DateTime fin,
            [FromQuery] List<string> salasIds,
            [FromQuery] string rrule)
        {
            var query = new ValidarRecurrenciaEventoQuery(
                inicio,
                fin,
                salasIds,
                rrule);

            var resultado = await _commandQueryBus.Send(query);

            return Ok(resultado);
        }

        ///Configurar salas para Eventos///

        [HttpPost("salas/{salaId}/configurar-actividades")]
        public async Task<IActionResult> ConfigurarSalaActividades(
            string salaId,
            [FromBody] ConfigurarSalaActividadesCommand command)
        {
            if (command is null)
                return BadRequest("El comando no puede ser nulo.");

            command.SalaId = salaId;
            var resultado = await _commandQueryBus.Send(command);
            return Ok(new { exito = resultado });
        }
        ///Reporte Eventos//

        [HttpGet("reporteEventos")]
        public async Task<IActionResult> GetReporteVisitaGuiada(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta)
        {
            if (desde == default || hasta == default)
            {
                return BadRequest("Parámetros 'desde' y 'hasta' requeridos en la query. Formato ISO: yyyy-MM-dd o yyyy-MM-ddTHH:mm:ss");
            }

            var reporte = await _commandQueryBus.Send(new EventsReportTableQuery(desde, hasta));

            return Ok(reporte);
        }
        ///Reporte Eventos por sala//

        [HttpGet("reporteEventoPorSala")]
        public async Task<IActionResult> GetReporteEventoPorSala(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta)
        {
            if (desde == default || hasta == default)
            {
                return BadRequest("Parámetros 'desde' y 'hasta' requeridos en la query. Formato ISO: yyyy-MM-dd o yyyy-MM-ddTHH:mm:ss");
            }

            var reporte = await _commandQueryBus.Send(new ReporteEventosPorSalaQuery(desde, hasta));

            return Ok(reporte);
        }
        ///Reporte Eventos por Tipo//


        [HttpGet("reporteEventoPorTipo")]
        public async Task<IActionResult> GetReporteEventoPorTipo(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta)
        {
            if (desde == default || hasta == default)
            {
                return BadRequest("Parámetros 'desde' y 'hasta' requeridos en la query. Formato ISO: yyyy-MM-dd o yyyy-MM-ddTHH:mm:ss");
            }

            var reporte = await _commandQueryBus.Send(new ReporteEventosPorTipoQuery(desde, hasta));

            return Ok(reporte);
        }
        ///Salas para eventos //

        [HttpGet("salas-disponibles-eventos")]
        public async Task<IActionResult> ObtenerSalasDisponiblesParaEvento()
        {
            var query = new SalaDispEventosQuery();
            var resultado = await _commandQueryBus.Send(query);
            return Ok(resultado);
        }


        private string BuildPublicImageUrl(string fileName)
        {
            var configuredBaseUrl = _configuration["Images:PublicBaseUrl"];
            var baseUrl = string.IsNullOrWhiteSpace(configuredBaseUrl)
                ? string.Empty
                : configuredBaseUrl.TrimEnd('/');

            if (!string.IsNullOrEmpty(baseUrl))
                return $"{baseUrl}/uploads/eventos/{fileName}";

            return $"/uploads/eventos/{fileName}";
        }



    }
}
