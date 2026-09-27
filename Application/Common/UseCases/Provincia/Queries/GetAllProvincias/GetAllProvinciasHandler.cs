using Application.ApplicationMuseo.DataTransferObjects;
using Application.Common.Repositories;
using Core.Application;
using Core.Application.Mapping;


namespace Application.ApplicationMuseo.UseCases.Provincia.Queries.GetAllProvincias
{
    internal sealed class GetAllProvinciasHandler(IProvinciaRepository context) : IRequestQueryHandler<GetAllProvinciasQuery, QueryResult<ProvinciaDto>>
    {
        private readonly IProvinciaRepository _context = context ?? throw new ArgumentNullException(nameof(context));
        public async Task<QueryResult<ProvinciaDto>> Handle(GetAllProvinciasQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.Common.Entities.Provincia> entities = await _context.FindAllAsync();
            return new QueryResult<ProvinciaDto>(entities.To<ProvinciaDto>(), entities.Count, request.PageIndex, request.PageSize);
        }
    }
}
