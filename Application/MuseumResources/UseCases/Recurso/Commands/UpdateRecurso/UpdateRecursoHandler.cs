using System;
using System.Threading.Tasks;
using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Core.Application;

namespace Application.MuseumResources.UseCases.Recurso.Commands.UpdateRecurso
{
    internal sealed class UpdateRecursoHandler(ICommandQueryBus domainBus, IRepositorioRecurso repositorioRecurso) : IRequestCommandHandler<UpdateRecursoCommand>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioRecurso _recursoRepository = repositorioRecurso ?? throw new ArgumentNullException(nameof(repositorioRecurso));

        public async Task Handle(UpdateRecursoCommand request, CancellationToken cancellationToken)
        {
            Domain.RecursoMuseo.Entities.Recurso entity = await _recursoRepository.FindOneAsync(request.Id) ?? throw new EntityDoesNotExistException();

            entity.ActualizarNombre(request.NombreRecurso);
            entity.ActualizarTipo(request.TipoRecurso);
            entity.ActualizarDescripcion(request.Descripcion);
            entity.CambiarEstado(request.Estado);
            entity.ActualizarCantidadTotal(request.CantidadTotal);

            try
            {
                _recursoRepository.Update(request.Id, entity);
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
