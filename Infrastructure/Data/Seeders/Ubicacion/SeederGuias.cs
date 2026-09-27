using Application.VisitaGrupal.Repositories;
using Domain.RecursoMuseo.Factories;
using Domain.RecursoMuseo.Other;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeders.Ubicacion
{
    public static class SeederGuias
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var repositorio = scope.ServiceProvider.GetRequiredService<IRepositorioGuia>();
            var logger = scope.ServiceProvider.GetService<ILogger<IRepositorioGuia>>();

            try
            {
                logger?.LogInformation("⏳ Verificando la existencia de guias en el sistema...");

                // 1. Verificar si ya existen salas en la Base de Datos para evitar duplicación
                // Nota: Asumo que tu repositorio tiene un método para contar o listar. Ajustar si es necesario.
                var guiasExistentes = await repositorio.FindAllAsync();

                if (guiasExistentes != null && guiasExistentes  .Any())
                {
                    logger?.LogInformation("✅ Los guias del museo ya se encuentran pobladas ({Count} encontradas). Seed omitido.", guiasExistentes.Count());
                    return;
                }

                logger?.LogInformation("🚀 No se encontraron guias. Iniciando el volcado de datos oficiales del Museo de Antropología...");
                // 2. Obtener el catálogo oficial validado desde la Factory de Dominio
                var guiasOficiales = GuiaFactory.CrearGuiasPorDefecto();

                // 3. Persistir cada guia de forma asíncrona mediante el repositorio
                foreach (var guia in guiasOficiales)
                {
                    await repositorio.AddAsync(guia);
                    logger?.LogInformation("   -> GUia registrada:{Nombre})",
                        guia.NombreCompleto);
                }

                logger?.LogInformation("🎉 Inyección de datos completada de manera exitosa. Se crearon {Count} salas.", guiasOficiales.Count);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "❌ Ocurrió un error crítico al intentar poblar las salas iniciales del museo.");
                throw;
            }
        }
    }
}