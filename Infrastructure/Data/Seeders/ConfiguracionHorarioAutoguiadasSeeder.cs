using Application.ConfiguracionSistema.Repositories;
using Application.VisitaGrupal.Repositories;
using Domain.VisitasGrupales.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeders
{
    public static class ConfiguracionHorarioAutoguiadasSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var repositorio = scope.ServiceProvider
                .GetRequiredService<IRepositorioConfiguracionHorarioAutoguiada>();

            var repositorioCalendario = scope.ServiceProvider
                .GetRequiredService<IRepositorioCalendarioMuseo>();

            var logger = scope.ServiceProvider
                .GetService<ILogger<IRepositorioConfiguracionHorarioAutoguiada>>();

            try
            {
                logger?.LogInformation(
                    "Verificando configuración de visitas autoguiadas...");

                var existente = await repositorio.ObtenerConfiguracionActivaAsync();

                if (existente != null)
                {
                    logger?.LogInformation(
                        "La configuración autoguiada ya existe. ID: {Id}",
                        existente.Id);

                    return;
                }

                var calendario = await repositorioCalendario
                    .ObtenerCalendarioActivoAsync();

                if (calendario == null)
                {
                    logger?.LogWarning(
                        "No se encontró un calendario activo del museo. " +
                        "No se puede crear la configuración autoguiada.");

                    return;
                }

                // Crear configuración usando el calendario actual del museo.
                var configuracionInicial =
                    ConfiguracionVisitasAutoguiadasFactory
                        .CrearConfiguracionPorDefecto(calendario);

                await repositorio.AddAsync(configuracionInicial);

                logger?.LogInformation(
                    "Configuración autoguiada creada correctamente.");

                logger?.LogInformation(
                    "Capacidad máxima por grupo: {Capacidad}",
                    configuracionInicial.CapacidadMaximaPorGrupo);

                logger?.LogInformation(
                    "Duración de visita: {Duracion}",
                    configuracionInicial.DuracionVisita);

                logger?.LogInformation(
                    "Intervalo entre reservas: {Intervalo}",
                    configuracionInicial.IntervaloReservas);

                logger?.LogInformation(
                    "Máximo de grupos simultáneos: {Simultaneas}",
                    configuracionInicial.VisitasSimultaneasMaximas);

                logger?.LogInformation(
                    "Capacidad total simultánea: {CapacidadTotal}",
                    configuracionInicial.CalcularCapacidadMaximaSimultanea());

                logger?.LogInformation(
                    "Cantidad de horarios configurados: {Cantidad}",
                    configuracionInicial.DiasDisponibles.Count);

                foreach (var horario in configuracionInicial.DiasDisponibles)
                {
                    logger?.LogInformation(
                        "Día: {Dia}; horario: {Desde} - {Hasta}",
                        horario.Dia,
                        horario.Desde,
                        horario.Hasta);
                }

                // Estimar los turnos para cada día configurado.
                var slotsPorDia = configuracionInicial.DiasDisponibles
                    .GroupBy(h => h.Dia)
                    .Select(grupo => new
                    {
                        Dia = grupo.Key,
                        Slots = grupo.Sum(h =>
                            CalcularSlotsEstimadosPorHorario(
                                h.Desde,
                                h.Hasta,
                                configuracionInicial.DuracionVisita,
                                configuracionInicial.IntervaloReservas))
                    });

                foreach (var item in slotsPorDia)
                {
                    logger?.LogInformation(
                        "Día: {Dia}; slots estimados: {Slots}",
                        item.Dia,
                        item.Slots);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(
                    ex,
                    "Error al sembrar la configuración autoguiada.");
            }
        }

        private static int CalcularSlotsEstimadosPorHorario(
            TimeOnly desde,
            TimeOnly hasta,
            TimeSpan duracionVisita,
            TimeSpan intervaloReservas)
        {
            var tiempoDisponible = hasta.ToTimeSpan() - desde.ToTimeSpan();

            if (duracionVisita <= TimeSpan.Zero ||
                intervaloReservas <= TimeSpan.Zero ||
                duracionVisita > tiempoDisponible)
            {
                return 0;
            }

            return (int)Math.Floor(
                (tiempoDisponible - duracionVisita).TotalMinutes /
                intervaloReservas.TotalMinutes) + 1;
        }
    }
}