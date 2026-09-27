using Application.VisitaGrupal.Repositories;

using Core.Application;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Application.VisitaGrupal.UseCases.Comands.CancelarVisitaGuiada
{
    internal sealed class CancelarVisitaGuiadaHanlder(IRepositorioVisitaGuiada repositorioVisitaGuiada) : IRequestCommandHandler<CancelarVisitaGuiadaCommand>
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        public async Task Handle(CancelarVisitaGuiadaCommand request, CancellationToken cancellationToken)
        {
            VisitaGrupalGuiada visita = await _repositorioVisitaGuiada.FindByIdWithActividadAsync(request.ReservationId);
            visita.CancelarVisitaGuiada();
            _repositorioVisitaGuiada.Update(request.ReservationId,visita);


        }
    
    }
}
