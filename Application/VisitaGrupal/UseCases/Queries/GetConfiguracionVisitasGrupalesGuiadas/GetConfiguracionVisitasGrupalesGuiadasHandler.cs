using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetConfiguracionVisitasGrupalesGuiadas
{
    internal sealed class GetConfiguracionVisitasGrupalesGuiadasHandler(IRepositorioConfiguracionVisitasGrupalesGuiadas repositorio) 
        : IRequestQueryHandler<GetConfiguracionVisitasGrupalesGuiadasQuery, ConfiguracionVisitasGrupalesGuiadasDto>
    {
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));

        public async Task<ConfiguracionVisitasGrupalesGuiadasDto> Handle(GetConfiguracionVisitasGrupalesGuiadasQuery request, CancellationToken cancellationToken)
        {
            var config = await _repositorio.ObtenerConfiguracionActivaAsync()
                ?? throw new InvalidOperationException("No existe una configuración activa.");

            return new ConfiguracionVisitasGrupalesGuiadasDto
            {
                Id = config.Id,
                CapacidadPorGuia = config.CapacidadPorGuia,
                MaximoVisitasSimultaneas= config.MaximoVisitasSimultaneas,
                CapacidadMaximaPorTurno = config.CapacidadMaximaPorTurno,
                DiasDisponibles = config.DiasDisponibles.Dias.ToList(),
                Turnos = config.Turnos.Select(t => new TurnoDto
                {
                    HoraInicio = t.HoraInicio,
                    HoraFin = t.HoraFin
                }).ToList()
            };
        }
    }
}
