using Application.VisitaGrupal.ApplicationServices;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Adapters
{
    internal sealed class HangfireGuidedTourExpirationScheduler(
        IBackgroundJobClient backgroundJobClient)
        : IGuidedTourConfirmationExpired
    {
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;

        public Task CancelGuidedTourAsync(string reservaId, DateTime fechaLimite)
        {
            _backgroundJobClient.Schedule<IVisitaGuiadaSinConfirmacion>(
                job => job.CancelarVisitaGuiadaSinConfirmacion(reservaId),
                fechaLimite);
            return Task.CompletedTask;
        }

        
    }
}
