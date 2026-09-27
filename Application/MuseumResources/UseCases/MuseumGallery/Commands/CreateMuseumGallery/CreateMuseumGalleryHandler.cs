using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;

using Core.Application;

namespace Application.MuseumResources.UseCases.MuseumGallery.Commands.CreateMuseumGallery
{
    internal sealed class CreateMuseumGalleryHandler(ICommandQueryBus domainBus, IRepositorioSala repositorioSala) : IRequestCommandHandler<CreateMuseumGalleryCommand, string>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioSala _context = repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));
       // private readonly IMuseumGalleryApplicationService _museumGalleryApplicationService = museumGalleryApplicationService ?? throw new ArgumentNullException(nameof(museumGalleryApplicationService));
        public Task<string> Handle(CreateMuseumGalleryCommand request, CancellationToken cancellationToken)
        {
            Domain.RecursoMuseo.Entities.Sala entity = new(request.Nombre, request.TipoSala, request.Capacidad,  request.Ubicacion,request.CodigoSala);
            //if (_museumGalleryApplicationService.MuseumGalleryExist(entity.Id)) throw new EntityDoesExistException();
            try
            {
                object createdId = _context.Add(entity);
                //_domainBus.Publish(entity.To<MuseumGalleryCreated>(), cancellationToken);
                return Task.FromResult(createdId.ToString());
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}


