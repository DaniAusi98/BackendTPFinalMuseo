using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.VisitaGrupal.Repositories;
using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Comands.CreateGuia
{
    internal sealed class CreateGuiaHandler(ICommandQueryBus domainBus, IRepositorioGuia repositorioGuia) : IRequestCommandHandler<CreateGuiaCommand, string>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioGuia _repositorioGuia = repositorioGuia ?? throw new ArgumentNullException(nameof(repositorioGuia));

        public Task<string> Handle(CreateGuiaCommand request, CancellationToken cancellationToken)
        {
            // Convert incoming DTO horarios to domain HorarioGuia
            var horarios = request.Horarios
                .Select(h => new Domain.RecursoMuseo.Entities.Guia.HorarioGuia(
                    new Domain.RecursoMuseo.ValueObjets.DiaLaboral(h.DiaAsignado),
                    h.HoraInicio,
                    h.HoraFin))
                .ToList();

            var entity = new Domain.RecursoMuseo.Entities.Guia.Guia(request.NombreCompleto, request.PersonalInternoId, horarios);

            try
            {
                object createdId = _repositorioGuia.Add(entity);
                // _domainBus.Publish(entity.To<GuiaCreada>(), cancellationToken);
                return Task.FromResult(createdId.ToString());
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
