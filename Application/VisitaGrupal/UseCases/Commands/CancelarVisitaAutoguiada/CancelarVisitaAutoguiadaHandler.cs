using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.VisitasGrupales.Entities.GrupalAutoguiada;

namespace Application.VisitaGrupal.UseCases.Commands.CancelarVisitaAutoguiada
{
    internal sealed class CancelarVisitaAutoGuiadaHanlder(IRepositorioVisitaGrupalAutoguiada repositorioVisitaAutoGuiada) : IRequestCommandHandler<CancelarVisitaAutoGuiadaCommand>
    {
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada = repositorioVisitaAutoGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaAutoGuiada));
        public async Task Handle(CancelarVisitaAutoGuiadaCommand request, CancellationToken cancellationToken)
        {
            VisitaGrupalAutoguiada visita = await _repositorioVisitaGrupalAutoguiada.FindByIdWithActividadAsync(request.ReservationId);
            visita.CancelarActividad();
            _repositorioVisitaGrupalAutoguiada.Update(request.ReservationId, visita);


        }

    }
}
