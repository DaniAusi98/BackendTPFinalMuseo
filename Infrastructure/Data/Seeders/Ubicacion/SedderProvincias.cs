using Application.Common.ApplicationServices; // Namespace donde esté IConsultarProvinciasArgetina
using Application.Common.Repositories;
using Application.Repositories; // Namespace de tus repositorios
using Domain.Common.Entities; // Namespace de la entidad Provincia
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeders.Ubicacion
{
    public static class ProvinciaSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            // 1. Resolvemos el Adaptador HTTP (la API) y tu Repositorio de persistencia en BD
            var apiService = scope.ServiceProvider.GetRequiredService<IConsultarProvinciasArgetina>();
            var repositorio = scope.ServiceProvider.GetRequiredService<IProvinciaRepository>();
            var logger = scope.ServiceProvider.GetService<ILogger<IProvinciaRepository>>();

            try
            {
                logger?.LogInformation(" Verificando catálogo de provincias en la base de datos...");

                // 2. Verificar si ya existen provincias cargadas para no duplicar datos
                var provinciasExistentes = await repositorio.FindAllAsync(); // O el método equivalente de tu interfaz

                if (provinciasExistentes != null && provinciasExistentes.Any())
                {
                    logger?.LogInformation(" El catálogo de provincias ya se encuentra poblado. Seed omitido.");
                    return;
                }

                logger?.LogInformation(" Conectando con la API de GeoRef para obtener provincias...");

                // 3. Consumir la API pública (aquí se ejecuta la llamada HTTP y el mapeo a dominio automáticamente)
                List<Provincia> provinciasApi = await apiService.ObtenerProvinciasArgentinasAsync();

                if (provinciasApi == null || !provinciasApi.Any())
                {
                    logger?.LogWarning(" La API externa no retornó provincias. No se puede poblar la base de datos.");
                    return;
                }

                logger?.LogInformation(" Se obtuvieron {Count} provincias de la API. Guardando en la base de datos...", provinciasApi.Count);

                // 4. Guardar la lista de entidades puras de dominio en tu base de datos
                await repositorio.AddRangeAsync(provinciasApi); // O tu método AddAsync/AddRange equivalente

                logger?.LogInformation("  Catálogo de provincias poblado correctamente en la base de datos.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "  Error crítico al ejecutar el seeder de provincias desde la API externa.");
                throw;
            }
        }
    }
}
