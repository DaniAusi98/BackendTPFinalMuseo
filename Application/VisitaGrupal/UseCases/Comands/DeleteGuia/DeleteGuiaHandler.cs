using System;
using System.Threading.Tasks;
using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;

using MediatR;

namespace Application.VisitaGrupal.UseCases.Comands.DeleteGuia
{
    internal sealed class DeleteGuiaHandler(ICommandQueryBus domainBus, IRepositorioGuia repositorioGuia)
        : IRequestCommandHandler<DeleteGuiaCommand,Unit>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioGuia _context = repositorioGuia ?? throw new ArgumentNullException(nameof(repositorioGuia));

        public Task<Unit> Handle(DeleteGuiaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Remove(request.GuiaId);
                return Unit.Task;
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
