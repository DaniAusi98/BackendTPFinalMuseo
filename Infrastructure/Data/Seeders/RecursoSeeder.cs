using Application.MuseumResources.Repositories;
using Domain.RecursoMuseo.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeders
{
    public static class RecursoSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var repositorio =
                scope.ServiceProvider.GetRequiredService<IRepositorioRecurso>();

            var logger =
                scope.ServiceProvider.GetService<ILogger<IRepositorioRecurso>>();

            try
            {
                logger?.LogInformation(
                    "Verificando recursos del museo...");

                // Verificar si ya existen recursos
                var existentes = await repositorio.FindAllAsync();

                if (existentes.Any())
                {
                    logger?.LogInformation(
                        "Ya existen recursos en la base de datos ({Cantidad}). Seed omitido.",
                        existentes.Count);

                    return;
                }

                // Crear recursos iniciales mediante Factory
                var recursosIniciales =
                    RecursoFactory.CrearRecursosPorDefecto();

                // Guardar todos los recursos
                
                    await repositorio.AddRangeAsync(recursosIniciales);
                

                logger?.LogInformation(
                    "Se crearon correctamente {Cantidad} recursos iniciales.",
                    recursosIniciales.Count);

                logger?.LogInformation(
                    " - Mobiliario: {Cantidad}",
                    recursosIniciales.Count(r =>
                        r.TipoRecurso == Domain.RecursoMuseo.Enums.Enums.TipoRecurso.Mobiliario));

                logger?.LogInformation(
                    " - Tecnológico: {Cantidad}",
                    recursosIniciales.Count(r =>
                        r.TipoRecurso == Domain.RecursoMuseo.Enums.Enums.TipoRecurso.Tecnologico));

                logger?.LogInformation(
                    " - Audiovisual: {Cantidad}",
                    recursosIniciales.Count(r =>
                        r.TipoRecurso == Domain.RecursoMuseo.Enums.Enums.TipoRecurso.Audiovisual));
            }
            catch (Exception ex)
            {
                logger?.LogError(
                    ex,
                    "Error al crear los recursos iniciales del museo.");

                throw;
            }
        }
    }
}