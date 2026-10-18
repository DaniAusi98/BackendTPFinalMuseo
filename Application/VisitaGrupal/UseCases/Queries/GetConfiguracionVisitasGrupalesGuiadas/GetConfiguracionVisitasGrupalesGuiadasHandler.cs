
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetConfiguracionVisitasGrupalesGuiadas
{
    internal sealed class GetConfiguracionVisitasGrupalesGuiadasHandler(
        IRepositorioConfiguracionVisitasGrupalesGuiadas repositorio)
        : IRequestQueryHandler<
            GetConfiguracionVisitasGrupalesGuiadasQuery,
            ConfiguracionVisitasGrupalesGuiadasDto>
    {
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorio =
            repositorio ?? throw new ArgumentNullException(nameof(repositorio));

        public async Task<ConfiguracionVisitasGrupalesGuiadasDto> Handle(
            GetConfiguracionVisitasGrupalesGuiadasQuery request,
            CancellationToken cancellationToken)
        {
            var config = await _repositorio.ObtenerConfiguracionActivaAsync()
                ?? throw new InvalidOperationException(
                    "No existe una configuración activa.");

            return new ConfiguracionVisitasGrupalesGuiadasDto
            {
                Id = config.Id,
                CapacidadPorGuia = config.CapacidadPorGuia,
                CapacidadMaximaPorTurno = config.CapacidadMaximaPorTurno,
                MaximoVisitasSimultaneas = config.MaximoVisitasSimultaneas,

                DiasDisponibles = config.DiasDisponibles
                    .Select(h => new HorarioDisponibleDto
                    {
                        Dia = h.Dia,
                        Desde = h.Desde,
                        Hasta = h.Hasta
                    })
                    .ToList()
            };
        }
    }
}
