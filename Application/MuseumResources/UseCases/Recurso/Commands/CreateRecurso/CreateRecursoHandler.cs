using System;
using System.Threading.Tasks;
using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Core.Application;

namespace Application.MuseumResources.UseCases.Recurso.Commands.CreateRecurso
{
    internal sealed class CreateRecursoHandler(ICommandQueryBus domainBus, IRepositorioRecurso repositorioRecurso) : IRequestCommandHandler<CreateRecursoCommand, string>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioRecurso _context = repositorioRecurso ?? throw new ArgumentNullException(nameof(repositorioRecurso));

        public Task<string> Handle(CreateRecursoCommand request, CancellationToken cancellationToken)
        {
            Domain.RecursoMuseo.Entities.Recurso entity = new(request.NombreRecurso, request.TipoRecurso, request.Descripcion, request.CantidadTotal);
            try
            {
                object createdId = _context.Add(entity);
                // _domainBus.Publish(entity.To<RecursoCreado>(), cancellationToken);
                return Task.FromResult(createdId.ToString());
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
