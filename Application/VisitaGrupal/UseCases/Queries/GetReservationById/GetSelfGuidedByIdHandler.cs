using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;


namespace Application.VisitaGrupal.UseCases.Queries.GetReservationById
{
    internal sealed class GetSelfGuidedByIdHandler(IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada) : IRequestQueryHandler<GetSelfGuidedByIdQuery, VisitaAutoguiadaDto>
    {
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada = repositorioVisitaGrupalAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));
        public async Task<VisitaAutoguiadaDto> Handle(GetSelfGuidedByIdQuery request, CancellationToken cancellationToken)
        {
            var visitaAutoguiada =await _repositorioVisitaGrupalAutoguiada.FindOneAsync(request.VisitaId);

            return visitaAutoguiada == null
                ? throw new Exception($"No se encontró la visita autoguiada con ID: {request.VisitaId}")
                : visitaAutoguiada.To<VisitaAutoguiadaDto>();
        }
    }
}
