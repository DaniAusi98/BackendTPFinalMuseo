using Application.ActividadMuseo.Repositories;
using Application.VisitaGrupal.Repositories;
using Domain.VisitasGrupales.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeders
{
    public static class ConfiguracionVisitasSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var repositorio = scope.ServiceProvider.GetRequiredService<IRepositorioConfiguracionVisitasGrupalesGuiadas>();
            var repositorioCalendario = scope.ServiceProvider.GetRequiredService<IRepositorioCalendarioMuseo>();
            var logger = scope.ServiceProvider.GetService<ILogger<IRepositorioConfiguracionVisitasGrupalesGuiadas>>();

            try
            {
                logger?.LogInformation(" Verificando configuración de visitas grupales guiadas...");

                // Verificar si ya existe configuración
                var existente = await repositorio.ObtenerConfiguracionActivaAsync();

                if (existente != null)
                {
                    logger?.LogInformation(" Configuración de visitas grupales guiadas ya existe (ID: {Id}). Seed omitido.", existente.Id);
                    return;
                }

                // Obtener el calendario del museo
                var calendario = await repositorioCalendario.ObtenerCalendarioActivoAsync();

                if (calendario == null)
                {
                    logger?.LogWarning(" No se encontró el calendario del museo. No se puede crear configuración de visitas guiadas.");
                    return;
                }

                logger?.LogInformation(" Calendario del museo encontrado (ID: {Id}). Creando configuración inicial...", calendario.Id);

                // Crear configuración por defecto usando el Factory (ahora con calendario)
                var configuracionInicial = ConfiguracionVisitasFactory.CrearConfiguracionPorDefecto(calendario);

                // Guardar en BD
                await repositorio.AddAsync(configuracionInicial);

                logger?.LogInformation("  Configuración de visitas grupales guiadas creada correctamente:");
                logger?.LogInformation("   - Capacidad por guía: {CapacidadPorGuia}", configuracionInicial.CapacidadPorGuia);
                logger?.LogInformation("   - Capacidad máxima: {CapacidadMaxima}", configuracionInicial.CapacidadMaximaPorTurno);
                logger?.LogInformation("   - Días disponibles: {DiasCount} de {DiasMuseo}",
                    configuracionInicial.DiasDisponibles.Dias.Count,
                    calendario.DiasApertura.Dias.Count);
                logger?.LogInformation("   - Turnos configurados: {TurnosCount}", configuracionInicial.Turnos.Count);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "  Error al crear configuración inicial de visitas grupales guiadas.");
                throw;
            }
        }
    }
}