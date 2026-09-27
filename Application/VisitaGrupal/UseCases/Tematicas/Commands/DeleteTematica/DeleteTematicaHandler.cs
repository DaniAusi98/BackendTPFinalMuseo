using System;
using System.Threading.Tasks;

using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.Repositories;

using Core.Application;

using MediatR;

namespace Application.VisitaGrupal.UseCases.Tematicas.Commands.DeleteTematica
{
    internal sealed class DeleteTematicaHandler(ICommandQueryBus domainBus, IRepositorioTematicas repositorioTematica)
        : IRequestCommandHandler<DeleteTematicaCommand, Unit>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioTematicas _context = repositorioTematica ?? throw new ArgumentNullException(nameof(repositorioTematica));

        public Task<Unit> Handle(DeleteTematicaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Remove(request.TematicaId);
                return Unit.Task;
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
