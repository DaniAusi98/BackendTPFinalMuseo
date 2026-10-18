using Application.VisitaGrupal.Repositories;
using Domain.RecursoMuseo.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeders.Ubicacion
{
    public static class SeederGuias
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var repositorio = scope.ServiceProvider
                .GetRequiredService<IRepositorioGuia>();

            var logger = scope.ServiceProvider
                .GetService<ILogger<IRepositorioGuia>>();

            try
            {
                logger?.LogInformation(
                    "Verificando la existencia de guías en el sistema...");

                var guiasExistentes = await repositorio.FindAllAsync();

                if (guiasExistentes != null && guiasExistentes.Any())
                {
                    logger?.LogInformation(
                        "Los guías ya están registrados ({Count}). Seed omitido.",
                        guiasExistentes.Count());

                    return;
                }

                logger?.LogInformation(
                    "No se encontraron guías. Iniciando carga inicial...");

                var guiasOficiales = GuiaFactory.CrearGuiasPorDefecto();

                foreach (var guia in guiasOficiales)
                {
                    await repositorio.AddAsync(guia);

                    logger?.LogInformation(
                        "Guía registrada: {Nombre} {Apellido}",
                        guia.Nombre,
                        guia.Apellido);
                }

                logger?.LogInformation(
                    "Carga inicial completada. Se crearon {Count} guías.",
                    guiasOficiales.Count);
            }
            catch (Exception ex)
            {
                logger?.LogError(
                    ex,
                    "Ocurrió un error al poblar los guías iniciales.");

                throw;
            }
        }
    }
}