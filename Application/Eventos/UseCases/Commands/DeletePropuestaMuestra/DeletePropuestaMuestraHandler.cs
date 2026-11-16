using Application.Eventos.Repositories;
using Core.Application;
using MediatR;

namespace Application.Eventos.UseCases.Commands.DeletePropuestaMuestra
{
    internal class DeletePropuestaMuestraHandler
        (IRepositorioMuestraTemporal repositorioMuestra,
        IRepositorioPropuestaMuestra repositorioPropuestaMuestra) : IRequestCommandHandler<DeletePropuestaMuestraCommand, Unit>
    {
        private readonly IRepositorioMuestraTemporal _repositorioMuestra = repositorioMuestra ?? throw new ArgumentNullException(nameof(repositorioMuestra));

        private readonly IRepositorioPropuestaMuestra _repositorioPropuestaMuestra = repositorioPropuestaMuestra ?? throw new ArgumentNullException(nameof(repositorioPropuestaMuestra));


        public async Task<Unit> Handle(DeletePropuestaMuestraCommand request, CancellationToken cancellationToken)
        {
            var propuestaMuestra = await _repositorioPropuestaMuestra.FindIncludeByIdAsync(request.PropuestaId);

            var muestra = propuestaMuestra.MuestraTemporal;

            muestra.CancelarPropuesta();

            _repositorioMuestra.Update(muestra.Id, muestra);

            _repositorioPropuestaMuestra.Remove(propuestaMuestra.Id);


            return Unit.Value;

        }
    }
}


