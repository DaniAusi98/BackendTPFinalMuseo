using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;
using Domain.Common.Entities;

namespace Application.ConfiguracionSistema.UseCases.Commands.CreateClosyngDay
{
    internal sealed class CreateClosyngHandler(IRepositorioDiaCierreMuseo repositorioDiaCierreMuseo) : IRequestCommandHandler<CreateClosyngDayCommand, string>
    {
        private readonly IRepositorioDiaCierreMuseo _repositorioDiaCierreMuseo = repositorioDiaCierreMuseo ?? throw new ArgumentNullException(nameof(repositorioDiaCierreMuseo));
        public async Task<string> Handle(CreateClosyngDayCommand request, CancellationToken cancellationToken)
        {
            DiaCierreMuseo entity = new(request.Fecha, request.FechaHasta, request.Motivo, request.Observaciones);

            try
            {
                object createdId = await _repositorioDiaCierreMuseo.AddAsync(entity);

                //await _domainBus.Publish(entity.To<DummyEntityCreated>(), cancellationToken);

                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
