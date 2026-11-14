using Application.ActividadesEducativas.Repositories;
using Core.Application;

namespace Application.ActividadesEducativas.UseCases.Commands.CancelarActEducativa
{
    internal sealed class CancelarActAducativaHandler(IRepositorioActividadEducativa repositorioActividadEducativa) : IRequestCommandHandler<CancelarActEducativaCommand>
    {
        private readonly IRepositorioActividadEducativa _repositorioActividadEducativa = repositorioActividadEducativa ?? throw new ArgumentNullException(nameof(repositorioActividadEducativa));
        public async Task Handle(CancelarActEducativaCommand request, CancellationToken cancellationToken)
        {
            var actividadEducativa = await _repositorioActividadEducativa.FindOneAsync(request.ActEducativaId);

            actividadEducativa.CancelarActividad();
            _repositorioActividadEducativa.Update(actividadEducativa.Id, actividadEducativa);

        }
    }
}
