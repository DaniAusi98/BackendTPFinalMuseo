using Application.Common.ApplicationServices;
using Application.Eventos.DataTransferObjets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Adapters
{

    public class LocalStorageService(IWebHostEnvironment environment, IConfiguration configuration) : IStorageService
    {
        private readonly IWebHostEnvironment _environment = environment;
        private readonly IConfiguration _configuration = configuration; // Agregado para leer el appsettings.json

        public async Task<string> GuardarArchivoAsync(ArchivoNuevoDto archivo, string carpeta, CancellationToken cancellationToken)
        {
            if (archivo.Contenido.Length == 0) return string.Empty;

            var uploadsDirectory = Path.Combine(
                _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot"),
                "uploads",
                carpeta);

            Directory.CreateDirectory(uploadsDirectory);

            var extension = Path.GetExtension(archivo.Nombre);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsDirectory, fileName);

            await File.WriteAllBytesAsync(filePath, archivo.Contenido, cancellationToken);

            return BuildPublicImageUrl(fileName, carpeta);
        }

        private string BuildPublicImageUrl(string fileName, string carpeta)
        {
            var configuredBaseUrl = _configuration["Images:PublicBaseUrl"];
            var baseUrl = string.IsNullOrWhiteSpace(configuredBaseUrl)
                ? string.Empty
                : configuredBaseUrl.TrimEnd('/');

            if (!string.IsNullOrEmpty(baseUrl))
                return $"{baseUrl}/uploads/{carpeta}/{fileName}";

            return $"/uploads/{carpeta}/{fileName}";
        }

        public void EliminarArchivo(string urlArchivo, string carpeta)
        {
            if (string.IsNullOrWhiteSpace(urlArchivo)) return;

            try
            {
                // 1. Extraemos solo el nombre del archivo (ej: "un-guid.jpg") de la URL, 
                // sin importar si viene con "http://...", "https://..." o "/uploads/..."
                var fileName = Path.GetFileName(urlArchivo);

                // 2. Construimos la ruta física absoluta en el disco del servidor
                var filePath = Path.Combine(
                    _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot"),
                    "uploads",
                    carpeta,
                    fileName);

                // 3. Si el archivo existe físicamente, lo borramos
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"No se pudo eliminar el archivo físico: {ex.Message}");
            }
        }
    }
}
