using Application.MuseumResources.Repositories;
using Domain.RecursoMuseo.Entities;
using Microsoft.Extensions.DependencyInjection;
using static Domain.ActividadMuseo.Enums.Enums;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Data.Seeders
{
    public static class ConfiguracionUsoSalas
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IRepositorioConfiguracionSalaActividad>();
            var logger = scope.ServiceProvider.GetService<ILogger<IRepositorioConfiguracionSalaActividad>>();


            try
            {
                logger?.LogInformation("⏳ Verificando la existencia de uso de salas en el sistema...");

                var existeConfiguracion = await context.FindAllAsync();
                if (existeConfiguracion != null && existeConfiguracion.Any())
                {
                    logger?.LogInformation("✅ Los uso de salas del museo ya se encuentran pobladas ({Count} encontradas). Seed omitido.", existeConfiguracion.Count());

                    return; // Si ya existen configuraciones, no se realiza el seed
                }
                logger?.LogInformation("🚀 No se encontraron uso de salas. Iniciando el volcado de datos oficiales del Museo de Antropología...");

                var configuraciones = new List<ConfiguracionSalaActividad>
            {
                // Auditorio (ID: "sala-auditorio-001")
                new ConfiguracionSalaActividad(
                    salaId: "4d6e2750-be3b-4c43-ae08-e68b3afac8ad",
                    tipoActividad: TipoActividad.Evento,
                    habilitada: true,
                    capacidadMaxima: 100),


               
            };

                await context.AddRangeAsync(configuraciones);

                logger?.LogInformation("🎉 Inyección de datos completada de manera exitosa. Se crearon {Count} uso de salas.", configuraciones.Count);


            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "❌ Ocurrió un error crítico al intentar poblar los usos de salas.");
                throw;
            }
        }
    }
}