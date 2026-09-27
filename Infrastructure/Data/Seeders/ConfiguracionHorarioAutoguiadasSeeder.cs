using Application.ActividadMuseo.Repositories;
using Application.VisitaGrupal.Repositories;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities;
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
                    "🔍 Verificando configuración de visitas grupales autoguiadas...");

                // Verificar si ya existe configuración
                var existente = await repositorio.ObtenerConfiguracionActivaAsync();

                if (existente != null)
                {
                    logger?.LogInformation(
                        "✅ Configuración de visitas grupales autoguiadas ya existe (ID: {Id}). Seed omitido.",
                        existente.Id);
                    return;
                }

                // Obtener el calendario del museo
                var calendario = await repositorioCalendario.ObtenerCalendarioActivoAsync();

                if (calendario == null)
                {
                    logger?.LogWarning(
                        "⚠️ No se encontró el calendario del museo. No se puede crear configuración de visitas autoguiadas.");
                    return;
                }

                logger?.LogInformation(
                    "📅 Horario calendario: {Inicio} - {Fin}",
                    calendario.HorarioApertura.HoraInicio,
                    calendario.HorarioApertura.HoraFin);

                // Crear configuración por defecto
                var configuracionInicial =
                    ConfiguracionVisitasAutoguiadasFactory.CrearConfiguracionPorDefecto(calendario);

                await repositorio.AddAsync(configuracionInicial);

                logger?.LogInformation("✅ Configuración de visitas grupales autoguiadas creada correctamente:");
                logger?.LogInformation("   📊 Capacidad máxima por grupo: {CapacidadMaximaPorGrupo} personas",
                    configuracionInicial.CapacidadMaximaPorGrupo);
                logger?.LogInformation("   ⏱️ Duración de visita: {DuracionVisita}",
                    configuracionInicial.DuracionVisita);
                logger?.LogInformation("   🔄 Intervalo entre reservas: {IntervaloReservas}",
                    configuracionInicial.IntervaloReservas);
                logger?.LogInformation("   👥 Visitas simultáneas máximas: {VisitasSimultaneas} grupos",
                    configuracionInicial.VisitasSimultaneasMaximas);
                logger?.LogInformation("   🏢 Capacidad total simultánea: {CapacidadTotal} personas",
                    configuracionInicial.CalcularCapacidadMaximaSimultanea());
                logger?.LogInformation("   📆 Días disponibles: {DiasCount} de {DiasMuseo}",
                    configuracionInicial.DiasDisponibles.Dias.Count,
                    calendario.DiasApertura.Dias.Count);
                logger?.LogInformation("   🕒 Horario: {Inicio} - {Fin}",
                    configuracionInicial.HorarioDisponibleVisitaAutoguiadas.HoraInicio,
                    configuracionInicial.HorarioDisponibleVisitaAutoguiadas.HoraFin);

                // Calcular cantidad aproximada de slots por día
                var slotsEstimados = CalcularSlotsEstimadosPorDia(
                    configuracionInicial.HorarioDisponibleVisitaAutoguiadas,
                    configuracionInicial.DuracionVisita,
                    configuracionInicial.IntervaloReservas);

                logger?.LogInformation("   🎯 Slots aproximados por día: {SlotsEstimados}",
                    slotsEstimados);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex,
                    "❌ Error al sembrar la configuración de visitas grupales autoguiadas.");
            }
        }

        /// <summary>
        /// Calcula la cantidad aproximada de slots que se generarán por día.
        /// </summary>
        private static int CalcularSlotsEstimadosPorDia(
            Horario horario,
            TimeSpan duracionVisita,
            TimeSpan intervaloReservas)
        {
            var horaInicio = horario.HoraInicio.ToTimeSpan();
            var horaFin = horario.HoraFin.ToTimeSpan();
            var tiempoDisponible = horaFin - horaInicio;

            // Si duracionVisita > tiempoDisponible, no hay slots
            if (duracionVisita > tiempoDisponible)
                return 0;

            // Cantidad de slots = (Tiempo disponible - Duración) / Intervalo + 1
            var slots = (int)Math.Floor(
                (tiempoDisponible - duracionVisita).TotalMinutes /
                intervaloReservas.TotalMinutes) + 1;

            return Math.Max(0, slots);
        }
    }
}