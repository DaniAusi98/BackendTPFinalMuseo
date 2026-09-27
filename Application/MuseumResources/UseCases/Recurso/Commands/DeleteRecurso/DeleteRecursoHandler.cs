using System;
using System.Threading.Tasks;
using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Core.Application;

using MediatR;

namespace Application.MuseumResources.UseCases.Recurso.Commands.DeleteRecurso
{
    internal sealed class DeleteRecursoHandler(ICommandQueryBus domainBus, IRepositorioRecurso repositorioRecurso)
        : IRequestCommandHandler<DeleteRecursoCommand,Unit>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioRecurso _context = repositorioRecurso ?? throw new ArgumentNullException(nameof(repositorioRecurso));

        public Task<Unit> Handle(DeleteRecursoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Remove(request.RecursoId);
                return Unit.Task;
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
/* internal sealed class DeleteSalaHandler(ICommandQueryBus domainBus, IRepositorioSala repositorioSala)
        : IRequestCommandHandler<DeleteMuseumGalleryCommand, Unit>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioSala _context = repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));
        public Task<Unit> Handle(DeleteMuseumGalleryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Remove(request.MuseumGalleryId);
               // _domainBus.Publish(new MuseumGalleryDeleted(request.MuseumGalleryId), cancellationToken);
                return Unit.Task;
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }*/
