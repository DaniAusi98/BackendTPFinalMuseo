using Application.Reportes.ApplicationServices;
using Application.Reportes.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;

namespace Application.Reportes.UseCases.Queries.ReporteVisitaGrupal
{
    internal sealed class ReporteVisitasGrupalesHandler(
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada, 
        IRepositorioVisitaGuiada repositorioVisitaGuiada,
        IServicioReporteVisitasGrupales servicioReporteVisitasGrupales
        ) : IRequestQueryHandler<ReporteGeneralVisitaGrupalQuery, ReporteVisitasGrupalesDto>
    {
        private readonly IRepositorioVisitaGrupalAutoguiada 
            _repositorioVisitaGrupalAutoguiada= 
            repositorioVisitaGrupalAutoguiada 
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));

        private readonly IRepositorioVisitaGuiada 
            _repositorioVisitaGuiada= 
            repositorioVisitaGuiada 
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        private readonly IServicioReporteVisitasGrupales 
            _servicioReporteVisitasGrupales= 
            servicioReporteVisitasGrupales 
            ?? throw new ArgumentNullException(nameof(servicioReporteVisitasGrupales));

        public async Task<ReporteVisitasGrupalesDto> Handle(ReporteGeneralVisitaGrupalQuery request, CancellationToken cancellationToken)
        {
            var visitasguiadas = await _repositorioVisitaGuiada.GetAllGroupVisitAsync(request.Desde, request.Hasta);
            var visitasAutoguiadas = await _repositorioVisitaGrupalAutoguiada.GetAllGroupVisitAuAsync(request.Desde, request.Hasta);
            var reporte = _servicioReporteVisitasGrupales.GenerarReporteGeneralVisitasGrupales(
                visitasAutoguiadas,
                visitasguiadas
                );
            return reporte;
        }
    }
}


