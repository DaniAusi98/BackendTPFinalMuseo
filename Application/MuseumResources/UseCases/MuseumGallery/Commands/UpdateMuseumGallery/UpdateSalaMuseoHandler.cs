using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;

using Core.Application;

namespace Application.MuseumResources.UseCases.MuseumGallery.Commands.UpdateMuseumGallery
{

    internal sealed class UpdateSalaMuseoHandler(ICommandQueryBus domainBus, IRepositorioSala salaMuseoRepository) : IRequestCommandHandler<UpdateMuseumGalleryCommand>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioSala _salaMuseoRepository = salaMuseoRepository ?? throw new ArgumentNullException(nameof(salaMuseoRepository));

        public async Task Handle(UpdateMuseumGalleryCommand request, CancellationToken cancellationToken)
        {
            Domain.RecursoMuseo.Entities.Sala entity = await _salaMuseoRepository.FindOneAsync(request.Id) ?? throw new EntityDoesNotExistException();
            entity.ActualizarNombre(request.Nombre);
            entity.CambiarEstado(request.EstadoSala);
            entity.ActualizarTipoSala(request.TipoSala);
            entity.ActualizarCapacidad(request.Capacidad);
            entity.ActualizarUbicacion(request.UbicacionSala);
            try
            {
                _salaMuseoRepository.Update(request.Id, entity);
               // return _domainBus.Publish(entity.To<SalaMuseoActualizada>(), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
/*internal sealed class UpdateDummyEntityHandler(ICommandQueryBus domainBus, IDummyEntityRepository dummyEntityRepository) : IRequestCommandHandler<UpdateDummyEntityCommand>
{
    private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
    private readonly IDummyEntityRepository _context = dummyEntityRepository ?? throw new ArgumentNullException(nameof(dummyEntityRepository));

    public async Task Handle(UpdateDummyEntityCommand request, CancellationToken cancellationToken)
    {
        Domain.Common.Entities.DummyEntity entity = await _context.FindOneAsync(request.DummyIdProperty) ?? throw new EntityDoesNotExistException();
        entity.SetdummyPropertyOne(request.dummyPropertyOne);
        entity.SetdummyPropertyTwo(request.dummyPropertyTwo);

        try
        {
            _context.Update(request.DummyIdProperty, entity);

            await _domainBus.Publish(entity.To<DummyEntityUpdated>(), cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
        }
    }*/
