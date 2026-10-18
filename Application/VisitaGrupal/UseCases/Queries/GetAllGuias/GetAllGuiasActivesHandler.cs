using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DataTransferObjets;
using Application.VisitaGrupal.DataTransferObjets.Guias;
using Application.VisitaGrupal.Repositories;
using Application.VisitaGrupal.UseCases.Queries.GetAllGuias;
using Core.Application;
using Core.Application.Mapping;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;

internal class GetAllGuiasActivesHandler(
    IRepositorioGuia repositorioGuia,
    IIdentityService identityService)
    : IRequestQueryHandler<GetAllGuiasActivesQuery, List<GuiaUsuarioDto>>
{
    private readonly IRepositorioGuia _repositorioGuia = repositorioGuia
        ?? throw new ArgumentNullException(nameof(repositorioGuia));

    private readonly IIdentityService _identityService = identityService
        ?? throw new ArgumentNullException(nameof(identityService));

    public async Task<List<GuiaUsuarioDto>> Handle(
        GetAllGuiasActivesQuery request,
        CancellationToken cancellationToken)
    {
        List<Guia> entities = await _repositorioGuia.ObtenerGuiasActivas();

        List<GuiaDto> guiaDtos = [.. entities.To<GuiaDto>()];

        List<string> userIds = guiaDtos
            .Select(g => g.UserId)
            .Where(id => !string.IsNullOrEmpty(id))
            .Distinct()
            .ToList();

        List<UserDto> usuarios = await _identityService.FindByIdsAsync(userIds);

        var usuariosPorId = usuarios.ToDictionary(u => u.Id);

        List<GuiaUsuarioDto> resultado = guiaDtos
            .Select(guia => new GuiaUsuarioDto
            {
                Guia = guia,
                UserGuia = usuariosPorId.GetValueOrDefault(guia.UserId)
            })
            .ToList();

        return resultado;
    }
}