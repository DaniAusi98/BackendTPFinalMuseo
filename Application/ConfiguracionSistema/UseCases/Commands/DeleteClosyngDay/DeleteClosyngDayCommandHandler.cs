using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;
using MediatR;

namespace Application.ConfiguracionSistema.UseCases.Commands.DeleteClosyngDay
{
    internal class DeleteClosyngDayCommandHandler(IRepositorioDiaCierreMuseo repositorioDiaCierreMuseo) : IRequestCommandHandler<DeleteClosyngDayCommand, Unit>
    {
        private readonly IRepositorioDiaCierreMuseo _repositorioDiaCierre = repositorioDiaCierreMuseo ?? throw new ArgumentNullException(nameof(repositorioDiaCierreMuseo));
        public Task<Unit> Handle(DeleteClosyngDayCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _repositorioDiaCierre.Remove(request.Id);

                // _domainBus.Publish(new DummyEntityDeleted(request.DummyIdProperty), cancellationToken);

                return Unit.Task;

            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
