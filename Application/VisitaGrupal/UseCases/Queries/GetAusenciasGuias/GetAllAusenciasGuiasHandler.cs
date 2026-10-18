using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DataTransferObjets;
using Application.VisitaGrupal.DataTransferObjets.Guias;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;

namespace Application.VisitaGrupal.UseCases.Queries.GetAusenciasGuias
{
    internal class GetAllAusenciasGuiasHandler(
        IRepositorioAusenciaGuia repositorioAusenciaGuia,
        IIdentityService identityService)
        : IRequestQueryHandler<GetAllAusenciasGuiasQuery, QueryResult<AusenciaGuiaDto>>
    {
        private readonly IRepositorioAusenciaGuia _repositorioAusenciaGuia =
            repositorioAusenciaGuia
            ?? throw new ArgumentNullException(nameof(repositorioAusenciaGuia));

        private readonly IIdentityService _identityService = identityService
            ?? throw new ArgumentNullException(nameof(identityService));

        public async Task<QueryResult<AusenciaGuiaDto>> Handle(
            GetAllAusenciasGuiasQuery request,
            CancellationToken cancellationToken)
        {
            List<AusenciaGuia> entities =
                await _repositorioAusenciaGuia.GetAllWithGuiaAsync();

            List<string> userIds = entities
                .Where(a => a.Guia != null)
                .Select(a => a.Guia.UserId)
                .Where(id => !string.IsNullOrEmpty(id))
                .Distinct()
                .ToList();

            List<UserDto> usuarios =
                await _identityService.FindByIdsAsync(userIds);

            var usuariosPorId = usuarios.ToDictionary(u => u.Id);

            var resultado = entities
            .Select(a => new AusenciaGuiaDto(
                a.GuiaId,
                a.Guia?.Nombre ?? string.Empty,
                a.Guia?.Apellido ?? string.Empty,
                a.Guia?.Email?.ToString() ?? string.Empty,
                a.Guia?.Telefono?.ToString() ?? string.Empty,
                a.FechaDesde,
                a.FechaHasta,
                a.Motivo))
            .ToList();

            return new QueryResult<AusenciaGuiaDto>(
                resultado,
                resultado.Count,
                request.PageIndex,
                request.PageSize);
        }
    }
}