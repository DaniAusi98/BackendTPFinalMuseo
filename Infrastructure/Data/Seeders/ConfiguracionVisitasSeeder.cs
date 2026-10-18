using Application.ConfiguracionSistema.Repositories;
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

            var repositorio = scope.ServiceProvider
                .GetRequiredService<IRepositorioConfiguracionVisitasGrupalesGuiadas>();

            var repositorioCalendario = scope.ServiceProvider
                .GetRequiredService<IRepositorioCalendarioMuseo>();

            var logger = scope.ServiceProvider
                .GetService<ILogger<IRepositorioConfiguracionVisitasGrupalesGuiadas>>();

            try
            {
                logger?.LogInformation(
                    "Verificando configuración de visitas grupales guiadas...");

                var existente = await repositorio.ObtenerConfiguracionActivaAsync();

                if (existente != null)
                {
                    logger?.LogInformation(
                        "La configuración de visitas guiadas ya existe. ID: {Id}. Seed omitido.",
                        existente.Id);

                    return;
                }

                var calendario = await repositorioCalendario
                    .ObtenerCalendarioActivoAsync();

                if (calendario == null)
                {
                    logger?.LogWarning(
                        "No se encontró el calendario del museo. " +
                        "No se puede crear la configuración de visitas guiadas.");

                    return;
                }

                logger?.LogInformation(
                    "Calendario encontrado (ID: {Id}). Creando configuración inicial...",
                    calendario.Id);

                var configuracionInicial =
                    ConfiguracionVisitasFactory.CrearConfiguracionPorDefecto(calendario);

                await repositorio.AddAsync(configuracionInicial);

                logger?.LogInformation(
                    "Configuración de visitas grupales guiadas creada correctamente.");

                logger?.LogInformation(
                    "Capacidad por guía: {CapacidadPorGuia}",
                    configuracionInicial.CapacidadPorGuia);

                logger?.LogInformation(
                    "Capacidad máxima por turno: {CapacidadMaxima}",
                    configuracionInicial.CapacidadMaximaPorTurno);

                logger?.LogInformation(
                    "Máximo de visitas simultáneas: {Maximo}",
                    configuracionInicial.MaximoVisitasSimultaneas);

                logger?.LogInformation(
                    "Cantidad de horarios disponibles: {Cantidad}",
                    configuracionInicial.DiasDisponibles.Count);

                foreach (var horario in configuracionInicial.DiasDisponibles)
                {
                    logger?.LogInformation(
                        "Día: {Dia}; horario: {Desde} - {Hasta}",
                        horario.Dia,
                        horario.Desde,
                        horario.Hasta);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(
                    ex,
                    "Error al crear la configuración inicial de visitas grupales guiadas.");

                throw;
            }
        }
    }
}