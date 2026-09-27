using System;
using System.Threading.Tasks;
using Application.MuseumResources.DataTransferObjects;
using Application.MuseumResources.Repositories;
using Core.Application;
using Core.Application.Mapping;

namespace Application.MuseumResources.UseCases.Recurso.Queries.GetAllRecursos
{
    internal class GetAllRecursosHandler(IRepositorioRecurso repositorioRecurso) : IRequestQueryHandler<GetAllRecursosQuery, QueryResult<RecursoDto>>
    {
        private readonly IRepositorioRecurso _repositorioRecurso = repositorioRecurso ?? throw new ArgumentNullException(nameof(repositorioRecurso));
        public async Task<QueryResult<RecursoDto>> Handle(GetAllRecursosQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.RecursoMuseo.Entities.Recurso> recursos = await _repositorioRecurso.FindAllAsync();
            return new QueryResult<RecursoDto>(recursos.To<RecursoDto>(), recursos.Count, request.PageIndex, request.PageSize);
        }
    }
}
