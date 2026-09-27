using System;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.MuseumResources.DataTransferObjects;
using Application.MuseumResources.Repositories;
using Core.Application;
using Core.Application.Mapping;

namespace Application.MuseumResources.UseCases.Recurso.Queries.GetRecursoBy
{
    internal sealed class GetRecursoByHandler(IRepositorioRecurso repositorioRecurso) : IRequestQueryHandler<GetRecursoByQuery, RecursoDto>
    {
        private readonly IRepositorioRecurso _context = repositorioRecurso ?? throw new ArgumentNullException(nameof(repositorioRecurso));
        public async Task<RecursoDto> Handle(GetRecursoByQuery request, CancellationToken cancellationToken)
        {
            Domain.RecursoMuseo.Entities.Recurso entity = await _context.FindOneAsync(request.RecursoId) ?? throw new EntityDoesNotExistException();
            return entity.To<RecursoDto>();
        }
    }
}
