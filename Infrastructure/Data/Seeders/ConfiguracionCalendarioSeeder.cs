using Application.ActividadMuseo.Repositories;
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


                // Verificar si ya existe calendario
                var existente = await repositorio.ObtenerCalendarioActivoAsync();

                if (existente != null)
                {
                    logger?.LogInformation(
                        "Calendario del museo ya existe (ID: {Id}). Seed omitido.",
                        existente.Id);

                    return;
                }


                logger?.LogInformation(
                    "No existe calendario. Creando configuración inicial...");


                // Crear calendario usando Factory
                var calendarioInicial =
                    CalendarioMuseoFactory.CrearCalendarioPorDefecto();


                // Guardar
                await repositorio.AddAsync(calendarioInicial);


                logger?.LogInformation(
                    "Calendario del museo creado correctamente:");

                logger?.LogInformation(
                    " - Horario: {Inicio} - {Fin}",
                    calendarioInicial.HorarioApertura.HoraInicio,
                    calendarioInicial.HorarioApertura.HoraFin);

                logger?.LogInformation(
                    " - Días abiertos: {Dias}",
                    calendarioInicial.DiasApertura.Dias.Count);

                logger?.LogInformation(
                    " - Días de cierre iniciales: {Cierres}",
                    calendarioInicial.DiasCierre.Count);
            }
            catch (Exception ex)
            {
                logger?.LogError(
                    ex,
                    "Error al crear calendario inicial del museo.");

                throw;
            }
        }
    }
}