
using Application.ConfiguracionSistema.UseCases.Commands.SynchronizeHolidays;
using Core.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices
{
    internal sealed class SynchronizeHolidaysBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<SynchronizeHolidaysBackgroundService> logger)
        : BackgroundService
    {
        private static readonly TimeSpan Intervalo =
            TimeSpan.FromDays(5);

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            // Primera sincronización al iniciar el servicio.
            await SincronizarAsync(stoppingToken);

            using var timer = new PeriodicTimer(Intervalo);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await SincronizarAsync(stoppingToken);
            }
        }

        private async Task SincronizarAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();

                var commandBus = scope.ServiceProvider
                    .GetRequiredService<ICommandQueryBus>();

                var command = new SynchronizeHolidaysCommand
                {
                    Year = DateTime.Now.Year
                };

                await commandBus.Send(command);

                logger.LogInformation(
                    "Sincronización de feriados del año {Year} ejecutada.",
                    command.Year);
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error al sincronizar los feriados del museo.");
            }
        }
    }
}
