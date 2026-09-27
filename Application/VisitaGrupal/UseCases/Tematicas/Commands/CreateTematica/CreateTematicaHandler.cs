using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.Repositories;

using Core.Application;

namespace Application.VisitaGrupal.UseCases.Tematicas.Commands.CreateTematica
{
    internal sealed class CreateTematicaHandler(ICommandQueryBus domainBus, IRepositorioTematicas repositorioTematica, IRepositorioSala repositorioSala) : IRequestCommandHandler<CreateTematicaCommand, string>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioTematicas _repositorioTematica = repositorioTematica ?? throw new ArgumentNullException(nameof(repositorioTematica));
        private readonly IRepositorioSala _repositorioSala = repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));

        public async Task<string> Handle(CreateTematicaCommand request, CancellationToken cancellationToken)
        {
            // fetch salas
            var salas = new List<Domain.RecursoMuseo.Entities.Sala>();
            foreach (var salaId in request.SalaIds.Distinct())
            {
                var sala = await _repositorioSala.FindOneAsync(salaId);
                if (sala is null) throw new EntityDoesNotExistException();
                salas.Add(sala);
            }

            var entity = new Domain.VisitasGrupales.Entities.TematicaVisita(request.Nombre, request.Descripcion, salas);

            try
            {
                object createdId = _repositorioTematica.Add(entity);
                // _domainBus.Publish(entity.To<TematicaCreada>(), cancellationToken);
                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
