using Application.PersonalMuseo.Repositories;
using Domain.PersonalMuseo.Entities.UsuarioInterno;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeders
{
    public static class AreaPuestoSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {

            using var scope = serviceProvider.CreateScope();

            var repositorioAreaPuesto = scope.ServiceProvider.GetRequiredService<IRepositorioAreaPuesto>();
            var repositorioArea = scope.ServiceProvider.GetRequiredService<IRepositorioArea>();
            var repositorioPuesto = scope.ServiceProvider.GetRequiredService<IRepositorioPuesto>();
            var logger = scope.ServiceProvider.GetService<ILogger<IRepositorioAreaPuesto>>();

            try
            {
                logger?.LogInformation("Verificando áreas de puestos del museo...");


                var existingPuestos = await repositorioAreaPuesto.FindAllAsync();

                if (!existingPuestos.Any())
                {
                    IReadOnlyCollection<Area> areas = await repositorioArea.FindAllAsync();
                    IReadOnlyCollection<Puesto> puestos = await repositorioPuesto.FindAllAsync();

                    var defaultAreas = AreaPuestoFactory.CrearAsignacionesOficiales(areas, puestos);

                    await repositorioAreaPuesto.AddRangeAsync(defaultAreas);
                    logger?.LogInformation("Se crearon correctamente {Cantidad} puestos en áreas iniciales.", defaultAreas.Count);
                }
                else
                {
                    logger?.LogInformation("Ya existen puestos en áreas en la base de datos ({Cantidad}). Seed omitido.", existingPuestos.Count);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error al crear puestos iniciales del museo.");
                throw;
            }
        }
    }
}
