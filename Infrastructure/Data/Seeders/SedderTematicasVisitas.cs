
using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.Repositories;
using Domain.RecursoMuseo.Other;
using Domain.VisitasGrupales.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeders
{
    public static class TematicaVisitaSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var repositorioTematica = scope.ServiceProvider.GetRequiredService<IRepositorioTematicas>();
            var repositorioSala = scope.ServiceProvider.GetRequiredService<IRepositorioSala>();
            var logger = scope.ServiceProvider.GetService<ILogger<IRepositorioTematicas>>();

            try
            {
                logger?.LogInformation("⏳ Verificando la existencia de temáticas de visitas en la base de datos...");

                // 1. Evitar ejecuciones duplicadas (Idempotencia)
                var tematicasExistentes = await repositorioTematica.FindAllAsync();
                if (tematicasExistentes != null && tematicasExistentes.Any())
                {
                    logger?.LogInformation("✅ Las temáticas ya se encuentran pobladas ({Count} encontradas). Seed omitido.", tematicasExistentes.Count());
                    return;
                }

                // 2. Recuperar las salas cargadas previamente en el sistema
                logger?.LogInformation("🔍 Recuperando salas del museo para vincular a las temáticas...");
                var salasEnBaseDatos = (await repositorioSala.FindAllAsync())?.ToList();

                // Salvaguarda: Si el seeder de salas no corrió, usamos la factory de salas para no romper el flujo
                if (salasEnBaseDatos == null || !salasEnBaseDatos.Any())
                {
                    logger?.LogWarning("⚠️ No se encontraron salas en la BD. Usando catálogo en memoria de SalaFactory.");
                    salasEnBaseDatos = SalaFactory.CrearSalasOficialesMuseo();
                }

                logger?.LogInformation("🚀 Generando temáticas oficiales validadas contra las reglas de negocio...");

                // 3. Invocar la Factory de Dominio pasando las salas (Pueblos Originarios recibirá sus 2 salas aquí)
                var tematicasOficiales = TematicaVisitaFactory.CrearTematicasOficiales(salasEnBaseDatos);

                // 4. Persistir las temáticas mediante el repositorio
                foreach (var tematica in tematicasOficiales)
                {
                    await repositorioTematica.AddAsync(tematica);
                    logger?.LogInformation("   -> Temática registrada: '{Nombre}' con {SalasCount} salas vinculadas.",
                        tematica.Nombre,
                        tematica.Salas.Count);
                }

                logger?.LogInformation("🎉 Inyección de temáticas completada con éxito. Se crearon {Count} registros.", tematicasOficiales.Count);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "❌ Error crítico al intentar poblar las temáticas de visita del museo.");
                throw;
            }
        }
    }
}
