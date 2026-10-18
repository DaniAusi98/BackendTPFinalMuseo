using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.ValueObjets;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;

namespace Application.VisitaGrupal.UseCases.Guias.Commands.UpdateGuia
{
    internal sealed class UpdateGuiaHandler(
        ICommandQueryBus domainBus,
        IRepositorioGuia repositorioGuia)
        : IRequestCommandHandler<UpdateGuiaCommand>
    {
        private readonly ICommandQueryBus _domainBus =
            domainBus ?? throw new ArgumentNullException(nameof(domainBus));

        private readonly IRepositorioGuia _repositorioGuia =
            repositorioGuia ?? throw new ArgumentNullException(nameof(repositorioGuia));

        public async Task Handle(
            UpdateGuiaCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repositorioGuia.FindOneAsync(request.Id)
                ?? throw new EntityDoesNotExistException();

            var horarios = request.Horarios
                .Select(h => new HorarioGuia(
                    new DiaLaboral(h.DiaAsignado),
                    h.HoraInicio,
                    h.HoraFin))
                .ToList();

            entity.ActualizarDatosPersonales(
                request.Nombre,
                request.Apellido,
                new Email(request.Email),
                new Telefono(request.Telefono));

            entity.AsignarHorarios(horarios);

            if (request.Activo && !entity.Activo)
                entity.GuiaActivo();
            else if (!request.Activo && entity.Activo)
                entity.GuiaNoActivo();

            try
            {
                _repositorioGuia.Update(request.Id, entity);
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