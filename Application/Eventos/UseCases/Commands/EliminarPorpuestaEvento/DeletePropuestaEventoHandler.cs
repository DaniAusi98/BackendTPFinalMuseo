using Application.Eventos.Repositories;
using Core.Application;
using Domain.Eventos.Entities;
using MediatR;

namespace Application.Eventos.UseCases.Commands.EliminarPorpuestaEvento
{
    internal class DeletePropuestaEventoHandler
        (IRepositorioEvento repositorioEvento,
        IRepositorioPropuestaEvento repositorioPropuestaEvento) : IRequestCommandHandler<DeletePropuestaEventoCommand, Unit>
    {
        private readonly IRepositorioEvento _repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));

        private readonly IRepositorioPropuestaEvento _repositorioPropuestaEvento = repositorioPropuestaEvento ?? throw new ArgumentNullException(nameof(repositorioPropuestaEvento));


        public async Task<Unit> Handle(DeletePropuestaEventoCommand request, CancellationToken cancellationToken)
        {
            var propuestaEvento = await _repositorioPropuestaEvento.FindIncludeByIdAsync(request.PropuestaId);

            Evento evento = propuestaEvento.Evento;

            evento.CancelarEvento();

            _repositorioEvento.Update(evento.Id, evento);

            _repositorioPropuestaEvento.Remove(propuestaEvento.Id);


            return Unit.Value;

        }
    }
}
