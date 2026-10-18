using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.ValueObjets;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;

namespace Application.VisitaGrupal.UseCases.Guias.Commands.CreateGuia
{
    internal sealed class CreateGuiaHandler(
        ICommandQueryBus domainBus,
        IRepositorioGuia repositorioGuia)
        : IRequestCommandHandler<CreateGuiaCommand, string>
    {
        private readonly ICommandQueryBus _domainBus =
            domainBus ?? throw new ArgumentNullException(nameof(domainBus));

        private readonly IRepositorioGuia _repositorioGuia =
            repositorioGuia ?? throw new ArgumentNullException(nameof(repositorioGuia));

        public Task<string> Handle(
            CreateGuiaCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var horarios = request.Horarios
                    .Select(h => new HorarioGuia(
                        new DiaLaboral(h.DiaAsignado),
                        h.HoraInicio,
                        h.HoraFin))
                    .ToList();

                var entity = new Guia(
                    request.Nombre,
                    request.Apellido,
                    new Email(request.Email),
                    new Telefono(request.Telefono),
                    horarios);

                var createdId = _repositorioGuia.Add(entity);

                return Task.FromResult(createdId.ToString());
            }
            catch (Exception ex)
            {
                throw new BussinessException(
                    ApplicationConstants.PROCESS_EXECUTION_EXCEPTION,
                    ex.InnerException ?? ex);
            }
        }
    }
}