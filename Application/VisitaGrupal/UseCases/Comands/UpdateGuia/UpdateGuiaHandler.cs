using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.VisitaGrupal.Repositories;
using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Comands.UpdateGuia
{
    internal sealed class UpdateGuiaHandler(ICommandQueryBus domainBus, IRepositorioGuia repositorioGuia) : IRequestCommandHandler<UpdateGuiaCommand>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioGuia _repositorioGuia = repositorioGuia ?? throw new ArgumentNullException(nameof(repositorioGuia));

        public async Task Handle(UpdateGuiaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repositorioGuia.FindOneAsync(request.Id) ?? throw new EntityDoesNotExistException();

            // convert horarios
            var horarios = request.Horarios
                .Select(h => new Domain.RecursoMuseo.Entities.Guia.HorarioGuia(
                    new Domain.RecursoMuseo.ValueObjets.DiaLaboral(h.DiaAsignado),
                    h.HoraInicio,
                    h.HoraFin))
                .ToList();

            entity.ActualizarNombreCompleto(request.NombreCompleto);
            entity.SetPersonalInternoId(request.PersonalInternoId);
            entity.AsignarHorarios(horarios);

            if (request.Activo)
                entity.GuiaActivo();
            else
                entity.GuiaNoActivo();

            try
            {
                _repositorioGuia.Update(request.Id, entity);
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
