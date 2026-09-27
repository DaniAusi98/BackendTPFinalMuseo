using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.Repositories;

using Core.Application;

namespace Application.VisitaGrupal.UseCases.Tematicas.Commands.UpdateTematica
{
    internal sealed class UpdateTematicaHandler(ICommandQueryBus domainBus, IRepositorioTematicas repositorioTematica, IRepositorioSala repositorioSala) : IRequestCommandHandler<UpdateTematicaCommand>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioTematicas _repositorioTematica = repositorioTematica ?? throw new ArgumentNullException(nameof(repositorioTematica));
        private readonly IRepositorioSala _repositorioSala = repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));

        public async Task Handle(UpdateTematicaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repositorioTematica.FindOneAsync(request.Id) ?? throw new EntityDoesNotExistException();

            // fetch salas
            var salas = new List<Domain.RecursoMuseo.Entities.Sala>();
            foreach (var salaId in request.SalaIds.Distinct())
            {
                var sala = await _repositorioSala.FindOneAsync(salaId);
                if (sala is null) throw new EntityDoesNotExistException();
                salas.Add(sala);
            }

            entity.Actualizar(request.Nombre, request.Descripcion);

            // replace assigned salas
            entity.AsignarSalas(salas);

            if (request.Disponible)
                entity.MarcarComoDisponible();
            else
                entity.MarcarComoNoDisponible();

            try
            {
                _repositorioTematica.Update(request.Id, entity);
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
