using Application.Eventos.Repositories;
using Core.Application;

namespace Application.Eventos.UseCases.Commands.CancelarEvento
{
    internal sealed class CancelarEventoHandler(IRepositorioEvento repositorioEvento) : IRequestCommandHandler<CancelarEventoCommand>
    {
        private readonly IRepositorioEvento _repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        public async Task Handle(CancelarEventoCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repositorioEvento.FindOneAsync(request.EventoId);

            entity.CancelarEvento();
            _repositorioEvento.Update(entity.Id, entity);
        }
    }
}
