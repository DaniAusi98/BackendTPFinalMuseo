using Application.Common.ApplicationServices;
using Application.Common.Repositories;
using Domain.Common.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Data.Seeders.Ubicacion
{
    public static class SeederDepartamentos
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider) {

            using var scope = serviceProvider.CreateScope();
            var apiService = scope.ServiceProvider.GetRequiredService<IConsultaDepartamentos>();
            var repositorio = scope.ServiceProvider.GetRequiredService<IDepartamentoRepository>();
            var logger = scope.ServiceProvider.GetService<ILogger<IDepartamentoRepository>>();

            try
            {
                logger?.LogInformation(" Verificando catálogo de departamentos en la base de datos...");

                // 2. Verificar si ya existen departamentos cargados para no duplicar datos
                var departamentosExistentes = await repositorio.FindAllAsync(); // O el método equivalente de tu interfaz

                if (departamentosExistentes != null && departamentosExistentes.Any())
                {
                    logger?.LogInformation(" El catálogo de departamentos ya se encuentra poblado. Seed omitido.");
                    return;
                }

                logger?.LogInformation(" Conectando con la API de GeoRef para obtener departamentos...");
                // 3. Consumir la API pública (aquí se ejecuta la llamada HTTP y el mapeo a dominio automáticamente)
                List<Departamento> departamentosApi = await apiService.ObtenerDepartamentosArgentinaAsync();

                if (departamentosApi == null || !departamentosApi.Any())
                {
                    logger?.LogWarning(" La API externa no retornó departamentos. No se puede poblar la base de datos.");
                    return;
                }

                logger?.LogInformation(" Se obtuvieron {Count} departamentos de la API. Guardando en la base de datos...", departamentosApi.Count);
                // 4. Guardar la lista de entidades puras de dominio en tu base de datos
                await repositorio.AddRangeAsync(departamentosApi); // O tu método AddAsync/AddRange equivalente

                logger?.LogInformation("  Catálogo de departamentos poblado correctamente en la base de datos.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "  Error crítico al ejecutar el seeder de departamentos desde la API externa.");
                throw;
            }





        }
    }
}
