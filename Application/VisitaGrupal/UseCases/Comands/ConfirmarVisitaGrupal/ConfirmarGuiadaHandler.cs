using Application.VisitaGrupal.Repositories;
using Core.Application;


namespace Application.VisitaGrupal.UseCases.Comands.ConfirmarVisitaGrupal
{
    internal sealed class ConfirmarGuiadaHandler(IRepositorioVisitaGuiada repositorioVisitaGuiada) : IRequestCommandHandler<ConfirmarGuiadaCommand>
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        public async Task Handle(ConfirmarGuiadaCommand request, CancellationToken cancellationToken)
        {
            var guiada = await _repositorioVisitaGuiada.FindOneAsync(request.ReservationId) ?? throw new Exception($"No se encontró la visita guiada con ID {request.ReservationId}");
            guiada.Confirmar();
            _repositorioVisitaGuiada.Update(request.ReservationId, guiada);

        }
    }
}
