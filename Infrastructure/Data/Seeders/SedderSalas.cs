using Application.MuseumResources.Repositories;
using Domain.RecursoMuseo.Other;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeders
{
    public static class SalaSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var repositorio = scope.ServiceProvider.GetRequiredService<IRepositorioSala>();
            var logger = scope.ServiceProvider.GetService<ILogger<IRepositorioSala>>();

            try
            {
                logger?.LogInformation("⏳ Verificando la existencia de salas en el sistema...");

                // 1. Verificar si ya existen salas en la Base de Datos para evitar duplicación
                // Nota: Asumo que tu repositorio tiene un método para contar o listar. Ajustar si es necesario.
                var salasExistentes = await repositorio.FindAllAsync();

                if (salasExistentes != null && salasExistentes.Any())
                {
                    logger?.LogInformation("✅ Las salas del museo ya se encuentran pobladas ({Count} encontradas). Seed omitido.", salasExistentes.Count());
                    return;
                }

                logger?.LogInformation("🚀 No se encontraron salas. Iniciando el volcado de datos oficiales del Museo de Antropología...");

                // 2. Obtener el catálogo oficial validado desde la Factory de Dominio
                var salasOficiales = SalaFactory.CrearSalasOficialesMuseo();

                // 3. Persistir cada sala de forma asíncrona mediante el repositorio
                foreach (var sala in salasOficiales)
                {
                    await repositorio.AddAsync(sala);
                    logger?.LogInformation("   -> Sala registrada: [{Codigo}] {Nombre} (Capacidad: {Capacidad})",
                        sala.CodigoSala,
                        sala.Nombre,
                        sala.Capacidad);
                }

                logger?.LogInformation("🎉 Inyección de datos completada de manera exitosa. Se crearon {Count} salas.", salasOficiales.Count);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "❌ Ocurrió un error crítico al intentar poblar las salas iniciales del museo.");
                throw;
            }
        }
    }
}
