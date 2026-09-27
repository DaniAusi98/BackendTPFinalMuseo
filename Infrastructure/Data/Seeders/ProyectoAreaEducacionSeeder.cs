using Application.ActividadesEducativas.Repositories;
using Domain.ActividadesAreaEducacion.Other;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeders
{
    public static class ProyectoAreaEducacionSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var repositorio = scope.ServiceProvider
                .GetRequiredService<IRepositorioProyectoAreaEducacion>();

            var logger = scope.ServiceProvider
                .GetService<ILogger<IRepositorioProyectoAreaEducacion>>();

            try
            {
                logger?.LogInformation(
                    " Verificando la existencia de proyectos del área de Educación...");

                var proyectosExistentes = await repositorio.FindAllAsync();

                if (proyectosExistentes != null && proyectosExistentes.Any())
                {
                    logger?.LogInformation(
                        "Los proyectos del área de Educación ya se encuentran poblados ({Count} encontrados). Seed omitido.",
                        proyectosExistentes.Count());

                    return;
                }

                logger?.LogInformation(
                    " No se encontraron proyectos. Iniciando carga de proyectos del área de Educación...");

                var proyectos = ProyectoAreaEducacionFactory
                    .CrearProyectosOficialesMuseo();

                foreach (var proyecto in proyectos)
                {
                    await repositorio.AddAsync(proyecto);

                    logger?.LogInformation(
                        "   -> Proyecto registrado: {Nombre} ({Responsables} responsables)",
                        proyecto.Nombre,
                        proyecto.EquipoTrabajoResponsable.Count);
                }

                logger?.LogInformation(
                    " Se crearon {Count} proyectos del área de Educación.",
                    proyectos.Count);
            }
            catch (Exception ex)
            {
                logger?.LogError(
                    ex,
                    " Ocurrió un error al intentar poblar los proyectos del área de Educación.");

                throw;
            }
        }
    }
}