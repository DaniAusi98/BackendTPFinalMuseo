using System;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;

namespace Application.VisitaGrupal.UseCases.Queries.GetGuiaBy
{
    internal sealed class GetGuiaByHandler(IRepositorioGuia repositorioGuia) : IRequestQueryHandler<GetGuiaByQuery, GuiaDto>
    {
        private readonly IRepositorioGuia _context = repositorioGuia ?? throw new ArgumentNullException(nameof(repositorioGuia));
        public async Task<GuiaDto> Handle(GetGuiaByQuery request, CancellationToken cancellationToken)
        {
            Domain.RecursoMuseo.Entities.Guia.Guia entity = await _context.FindOneAsync(request.GuiaId) ?? throw new EntityDoesNotExistException();
            return entity.To<GuiaDto>();
        }
    }
}
