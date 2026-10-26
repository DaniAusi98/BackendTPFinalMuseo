using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.VisitasGrupales.Entities.GrupalAutoguiada;

namespace Application.VisitaGrupal.UseCases.Queries.GetConfiguracionVisitasAutoguiadas
{
    internal sealed class GetSettingSelfGuidedToursHandler(IRepositorioConfiguracionHorarioAutoguiada repositorioConfiguracionHorarioAutoguiada) : IRequestQueryHandler<GetSettingSelfGuidedToursQuery, ConfiguracionVisitasAutoguiadasDto>
    {
        private readonly IRepositorioConfiguracionHorarioAutoguiada _repositorioConfiguracionHorarioAutoguiada = repositorioConfiguracionHorarioAutoguiada ?? throw new ArgumentNullException(nameof(repositorioConfiguracionHorarioAutoguiada));
        public async Task<ConfiguracionVisitasAutoguiadasDto> Handle(GetSettingSelfGuidedToursQuery request, CancellationToken cancellationToken)
        {

            ConfiguracionHorarioAutoguiada configuracionHorarioAutoguiada = await _repositorioConfiguracionHorarioAutoguiada.ObtenerConfiguracionActivaAsync();
            return configuracionHorarioAutoguiada.To<ConfiguracionVisitasAutoguiadasDto>();


        }
    }
}
