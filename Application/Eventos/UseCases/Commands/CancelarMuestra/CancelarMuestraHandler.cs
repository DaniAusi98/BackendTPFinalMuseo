using Application.Eventos.Repositories;
using Core.Application;

namespace Application.Eventos.UseCases.Commands.CancelarMuestra
{
    internal sealed class CancelarMuestraHandler(IRepositorioMuestraTemporal repositorioMuestraTemporal) : IRequestCommandHandler<CancelarMuestraCommand>
    {
        private readonly IRepositorioMuestraTemporal _repositorioMuestraTemporal = repositorioMuestraTemporal;
        public async Task Handle(CancelarMuestraCommand request, CancellationToken cancellationToken)
        {
            var muestra = await _repositorioMuestraTemporal.FindOneAsync(request.MuestraId);

            muestra.CancelarPropuesta();
            _repositorioMuestraTemporal.Update(muestra.Id, muestra);


        }
    }
}
