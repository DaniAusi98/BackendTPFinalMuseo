using Application.PersonalMuseo.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeders
{
    public static class PuestoSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var repositorio =
            scope.ServiceProvider.GetRequiredService<IRepositorioPuesto>();
            var logger = scope.ServiceProvider.GetService<ILogger<IRepositorioPuesto>>();
            try
            {
                logger?.LogInformation(
                    "Verificando puestos del museo...");

                var existingPuestos = await repositorio.FindAllAsync();
                if (!existingPuestos.Any())
                {
                    var defaultPuestos = PuestoFactory.CrearPuestosOficiales();
                    await repositorio.AddRangeAsync(defaultPuestos);
                    logger?.LogInformation("Se crearon correctamente {Cantidad} puestos iniciales.", defaultPuestos.Count);
                }
                else
                {
                    logger?.LogInformation("Ya existen puestos en la base de datos ({Cantidad}). Seed omitido.", existingPuestos.Count);
                }

            }
            catch (Exception ex)
            {
                logger?.LogError(
                    ex,
                    "Error al crear los puestos iniciales del museo.");
                throw;
            }
        }
    }
}
