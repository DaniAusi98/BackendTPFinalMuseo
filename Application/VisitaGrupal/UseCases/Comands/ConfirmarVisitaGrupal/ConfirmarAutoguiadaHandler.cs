using Application.VisitaGrupal.Repositories;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Comands.ConfirmarVisitaGrupal
{
    internal sealed class ConfirmarAutoguiadaHandler(IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada): IRequestCommandHandler<ConfirmarAutoguiadaCommand>
    {
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada = repositorioVisitaGrupalAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));
        public async Task Handle(ConfirmarAutoguiadaCommand request, CancellationToken cancellationToken)
        {
            var autoguiada = await _repositorioVisitaGrupalAutoguiada.FindOneAsync(request.ReservationId) ?? throw new Exception($"No se encontró la visita autoguiada con ID {request.ReservationId}");
            autoguiada.Confirmar();
            _repositorioVisitaGrupalAutoguiada.Update(request.ReservationId, autoguiada);
        }
    
    }
}
