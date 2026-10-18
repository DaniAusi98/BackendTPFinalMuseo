using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;

namespace Application.VisitaGrupal.UseCases.Guias.Commands.CrearAusenciaGuia
{
    internal sealed class CrearAusenciaHandler(IRepositorioAusenciaGuia repositorioAusenciaGuia) : IRequestCommandHandler<CrearAusenciaCommand, string>
    {
        private readonly IRepositorioAusenciaGuia _repositorioAusenciaGuia = repositorioAusenciaGuia ?? throw new ArgumentNullException(nameof(repositorioAusenciaGuia));
        public async Task<string> Handle(CrearAusenciaCommand request, CancellationToken cancellationToken)
        {
            var entity = new AusenciaGuia(request.GuiaId, request.FechaDesde, request.FechaHasta, request.Motivo);

            try
            {
                object createdId = await _repositorioAusenciaGuia.AddAsync(entity);
                // _domainBus.Publish(entity.To<GuiaCreada>(), cancellationToken);
                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }


        }
    }
}
