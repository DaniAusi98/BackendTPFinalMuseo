using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;

using Core.Application;

using MediatR;

namespace Application.MuseumResources.UseCases.MuseumGallery.Commands.DeleteMuseumGallery
{
    internal sealed class DeleteSalaHandler(ICommandQueryBus domainBus, IRepositorioSala repositorioSala)
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
    }
    
    
}
/*internal sealed class DeleteDummyEntityHandler(ICommandQueryBus domainBus, IDummyEntityRepository dummyEntityRepository)
        : IRequestCommandHandler<DeleteDummyEntityCommand, Unit>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IDummyEntityRepository _context = dummyEntityRepository ?? throw new ArgumentNullException(nameof(dummyEntityRepository));

        public Task<Unit> Handle(DeleteDummyEntityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Remove(request.DummyIdProperty);

                _domainBus.Publish(new DummyEntityDeleted(request.DummyIdProperty), cancellationToken);

                return Unit.Task;
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }*/
