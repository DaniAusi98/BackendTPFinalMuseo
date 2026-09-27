using Application.ApplicationMuseo.ApplicationServices;
using Application.ApplicationMuseo.Constants;
using Application.ApplicationMuseo.DomainEvents;
using Application.Common.Repositories;
using Application.Exceptions;

using Core.Application;
using Core.Application.Mapping;

namespace Application.ApplicationMuseo.UseCases.DummyEntity.Commands.CreateDummyEntity
{
    /// <summary>
    /// Ejemplo de handler que responde al comando <see cref="CreateDummyEntityCommand"/>
    /// y ejecuta el proceso para el caso de uso en cuestion.
    /// Todo handler debe implementar la interfaz <see cref="IRequestCommandHandler{TRequest, TResponse}"/>
    /// si devuelve una respuesta donde <c TRequest> es del tipo <see cref="CreateDummyEntityCommand"/>
    /// y <c TResponse> del tipo de dato definido para la respuesta
    /// /// </summary>
    internal sealed class CreateDummyEntityHandler(ICommandQueryBus domainBus, IDummyEntityRepository dummyEntityRepository, IDummyEntityApplicationService dummyEntityApplicationService) : IRequestCommandHandler<CreateDummyEntityCommand, string>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IDummyEntityRepository _context = dummyEntityRepository ?? throw new ArgumentNullException(nameof(dummyEntityRepository));
        private readonly IDummyEntityApplicationService _dummyEntityApplicationService = dummyEntityApplicationService ?? throw new ArgumentNullException(nameof(dummyEntityApplicationService));
        public async Task<string> Handle(CreateDummyEntityCommand request, CancellationToken cancellationToken)
        {
            Domain.Common.Entities.DummyEntity entity = new(request.dummyPropertyOne, request.dummyPropertyTwo);


            if (_dummyEntityApplicationService.DummyEntityExist(entity.Id)) throw new EntityDoesExistException();

            try
            {
                object createdId = await _context.AddAsync(entity);

                await _domainBus.Publish(entity.To<DummyEntityCreated>(), cancellationToken);

                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
