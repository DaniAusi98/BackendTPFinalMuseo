using Application.Eventos.UseCases.Commands;
using Application.Eventos.UseCases.Queries;
using Application.Eventos.UseCases.Queries.ReporteEventos;
using Application.Eventos.UseCases.Queries.SalasDisponibles;
using Application.MuseumResources.UseCases.Commands.ConfigurarSalaActividades;

using Core.Application;
using Microsoft.AspNetCore.Mvc;
using System;

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

        // =========================================================
        // FUTURO: Endpoint para cuando se migre a Cloudflare R2
        // =========================================================
        // Requiere:
        // 1) Instalar AWSSDK.S3 (R2 es compatible con API S3).
        // 2) Registrar un IAmazonS3 configurado con:
        //    - ServiceURL: https://<ACCOUNT_ID>.r2.cloudflarestorage.com
        //    - Credenciales R2 (AccessKey / SecretKey)
        //    - ForcePathStyle = true

        //donde guardar todo eso ?

        //En appsettings.json(o mejor, en appsettings.Development.json / variables de entorno / secrets, nunca en el repo público):
        //"CloudflareR2": {
        //"AccountId": "xxxxxxxxxxxxxxxxxxxx",
        //"AccessKey": "xxxxxxxxxxxxxxxxxxxx",
        //"SecretKey": "xxxxxxxxxxxxxxxxxxxx",
        //"BucketName": "eventos-museo",
        //"PublicBaseUrl": "https://cdn.tudominio.com"
        //}
        // 3) Definir en appsettings:
        //    "CloudflareR2": {
        //        "BucketName": "eventos-museo",
        //        "PublicBaseUrl": "https://cdn.tudominio.com"
        //    }
        //
        // [HttpPost("r2")]
        // [Consumes("multipart/form-data")]
        // public async Task<IActionResult> CreateWithR2(
        //     [FromForm] CrearEventoCommand command,
        //     [FromForm] List<IFormFile>? imagenes,
        //     [FromServices] IAmazonS3 s3Client)
        // {
        //     if (command is null)
        //         return BadRequest();
        //
        //     var bucketName = _configuration["CloudflareR2:BucketName"];
        //     var publicBaseUrl = _configuration["CloudflareR2:PublicBaseUrl"]!.TrimEnd('/');
        //
        //     var urls = command.UrlImagenes?.ToList() ?? [];
        //
        //     if (imagenes is not null && imagenes.Count > 0)
        //     {
        //         foreach (var imagen in imagenes.Where(i => i.Length > 0))
        //         {
        //             var extension = Path.GetExtension(imagen.FileName);
        //             var key = $"eventos/{Guid.NewGuid()}{extension}";
        //
        //             await using var stream = imagen.OpenReadStream();
        //
        //             var putRequest = new PutObjectRequest
        //             {
        //                 BucketName = bucketName,
        //                 Key = key,
        //                 InputStream = stream,
        //                 ContentType = imagen.ContentType,
        //                 // R2 no soporta ACL público via header; el bucket
        //                 // debe estar configurado como público o servido
        //                 // detrás de un dominio custom (Cloudflare).
        //             };
        //
        //             await s3Client.PutObjectAsync(putRequest);
        //
        //             urls.Add($"{publicBaseUrl}/{key}");
        //         }
        //     }
        //
        //     command.UrlImagenes = urls;
        //
        //     var id = await _commandQueryBus.Send(command);
        //     return Created($"api/v1/eventos/{id}", new { Id = id });
        // }
    }
}
