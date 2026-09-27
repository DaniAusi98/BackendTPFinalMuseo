
using Application.ActividadesEducativas.DataTransferObjets;
using Application.ActividadesEducativas.Repositories;
using Core.Application;
using Core.Application.Mapping;

namespace Application.ActividadesEducativas.UseCases.Queries.ProyectosEducativos
{
    internal sealed class ObtenerProyectosEducativosHandler(
        IRepositorioProyectoAreaEducacion repositorioProyectoAreaEducacion) : IRequestQueryHandler
        <ObtenerProyectosEducativosQuery, List<ProyectoEducativoDto>>
    {
        private readonly IRepositorioProyectoAreaEducacion _repositorioProyectoAreaEducacion= repositorioProyectoAreaEducacion ?? throw new ArgumentNullException(nameof(repositorioProyectoAreaEducacion));
        public async Task<List<ProyectoEducativoDto>> Handle(ObtenerProyectosEducativosQuery request, CancellationToken cancellationToken)
        {
            var proyectosEducativos = await _repositorioProyectoAreaEducacion.FindAllAsyncIncludes();

            return [.. proyectosEducativos.To<ProyectoEducativoDto>()];
           
        }
    }
}
