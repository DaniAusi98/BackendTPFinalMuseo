using Application.Common.ApplicationServices;
using Application.Common.Repositories;
using Domain.Common.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeders.Ubicacion
{
    public static class SeederLocalidades
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {

            using var scope = serviceProvider.CreateScope();
            var apiService = scope.ServiceProvider.GetRequiredService<IConsultarLocalidades>();
            var repositorio = scope.ServiceProvider.GetRequiredService<ILocalidadRepository>();
            var logger = scope.ServiceProvider.GetService<ILogger<ILocalidadRepository>>();

            try
            {
                logger?.LogInformation(" Verificando catálogo de localidades en la base de datos...");

                // 2. Verificar si ya existen localidades cargadas para no duplicar datos
                var localidadesExistentes = await repositorio.FindAllAsync(); // O el método equivalente de tu interfaz

                if (localidadesExistentes != null && localidadesExistentes.Any())
                {
                    logger?.LogInformation(" El catálogo de localidades ya se encuentra poblado. Seed omitido.");
                    return;
                }

                logger?.LogInformation(" Conectando con la API de GeoRef para obtener localidades...");
                // 3. Consumir la API pública (aquí se ejecuta la llamada HTTP y el mapeo a dominio automáticamente)
                List<LocalidadArg> localidadesApi = await apiService.ObtenerLocalidadesArgentinaAsync();

                if (localidadesApi == null || !localidadesApi.Any())
                {
                    logger?.LogWarning(" La API externa no retornó localidades. No se puede poblar la base de datos.");
                    return;
                }

                logger?.LogInformation(" Se obtuvieron {Count} localidades de la API. Guardando en la base de datos...", localidadesApi.Count);
                // 4. Guardar la lista de entidades puras de dominio en tu base de datos
                await repositorio.AddRangeAsync(localidadesApi); // O tu método AddAsync/AddRange equivalente

                logger?.LogInformation("  Catálogo de localidades poblado correctamente en la base de datos.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "  Error crítico al ejecutar el seeder de localidades desde la API externa.");
                throw;
            }


        }
    }
}
