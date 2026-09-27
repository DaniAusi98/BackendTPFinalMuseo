using Application.VisitaGrupal.ApplicationServices;
using Application.VisitaGrupal.DomainEvents;
using Application.VisitaGrupal.Repositories;
using MediatR;

namespace Application.VisitaGrupal.UseCases.Notification
{
    internal class CancelarSinConfirmar(IGuidedTourConfirmationExpired guidedTourConfirmationExpired ,IRepositorioVisitaGuiada repositorioVisitaGuiada) : INotificationHandler<VisitaGuiadaCreated>
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        private readonly IGuidedTourConfirmationExpired _guidedTourConfirmationExpired = guidedTourConfirmationExpired ?? throw new ArgumentNullException(nameof(guidedTourConfirmationExpired));
        public Task Handle(VisitaGuiadaCreated notification, CancellationToken cancellationToken)
        {
            var fechaLimite= notification.Inicio.Subtract(TimeSpan.FromHours(48));


            _guidedTourConfirmationExpired.CancelGuidedTourAsync(notification.VisitaId, fechaLimite);

            return Task.CompletedTask;
        }
    }
}




