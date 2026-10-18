using Application.ConfiguracionSistema.Repositories;
using Domain.Common.Others.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeders
{
    public static class CalendarioMuseoSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var repositorio = scope.ServiceProvider
                .GetRequiredService<IRepositorioCalendarioMuseo>();

            var logger = scope.ServiceProvider
                .GetService<ILogger<IRepositorioCalendarioMuseo>>();

            try
            {
                logger?.LogInformation(
                    "Verificando calendario del museo...");

                var existente = await repositorio
                    .ObtenerCalendarioActivoAsync();

                if (existente != null)
                {
                    logger?.LogInformation(
                        "El calendario del museo ya existe (ID: {Id}). Seed omitido.",
                        existente.Id);

                    return;
                }

                logger?.LogInformation(
                    "No existe calendario. Creando configuración inicial...");

                var calendarioInicial =
                    CalendarioMuseoFactory.CrearCalendarioPorDefecto();

                await repositorio.AddAsync(calendarioInicial);

                logger?.LogInformation(
                    "Calendario del museo creado correctamente.");

                logger?.LogInformation(
                    "Cantidad de horarios de apertura: {Cantidad}",
                    calendarioInicial.HorariosApertura.Count);

                foreach (var horario in calendarioInicial.HorariosApertura)
                {
                    logger?.LogInformation(
                        "Día: {Dia}; horario: {Inicio} - {Fin}",
                        horario.Dia,
                        horario.HoraInicio,
                        horario.HoraFin);
                }

                logger?.LogInformation(
                    "Cantidad de cierres especiales: {Cierres}",
                    calendarioInicial.DiasCierre.Count);
            }
            catch (Exception ex)
            {
                logger?.LogError(
                    ex,
                    "Error al crear el calendario inicial del museo.");

                throw;
            }
        }
    }
}