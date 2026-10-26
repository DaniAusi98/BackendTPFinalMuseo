using Application.ConfiguracionSistema.DataTransferObjets;

namespace Application.ConfiguracionSistema.Repositories
{
    public interface IFeriadosProvider
    {
        Task<IReadOnlyCollection<FeriadoExternoDto>>
            ObtenerFeriadosAsync(
                int anio,
                CancellationToken cancellationToken);
    }
}