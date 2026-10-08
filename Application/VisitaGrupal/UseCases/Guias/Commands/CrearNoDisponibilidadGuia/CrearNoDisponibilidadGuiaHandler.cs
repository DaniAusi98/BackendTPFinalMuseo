using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.RecursoMuseo.Entities.Guia;


namespace Application.VisitaGrupal.UseCases.Guias.Commands.CrearNoDisponibilidadGuia
{
    internal sealed class CrearNoDisponibilidadGuiaHandler(IRepositorioAusenciaGuia repositorioAusenciaGuia) : IRequestCommandHandler<CrearNoDisponibilidadGuiaCommand, string>
    {
        private readonly IRepositorioAusenciaGuia _repositorioAusenciaGuia = repositorioAusenciaGuia ?? throw new ArgumentNullException(nameof(repositorioAusenciaGuia));
        public async Task<string> Handle(CrearNoDisponibilidadGuiaCommand request, CancellationToken cancellationToken)
        {
            var noDisponibilidad = new AusenciaGuia(request.GuiaId, request.FechaDesde, request.FechaHasta, request.Motivo);
            try
            {
                object createdId = await _repositorioAusenciaGuia.AddAsync(noDisponibilidad);
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
