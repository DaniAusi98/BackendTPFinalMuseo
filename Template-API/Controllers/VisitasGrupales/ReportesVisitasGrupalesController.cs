using Application.Reportes.UseCases.Queries.ReporteGrid;
using Application.Reportes.UseCases.Queries.ReporteVisitanteSala;
using Application.Reportes.UseCases.Queries.ReporteVisitaGrupal;
using Application.Reportes.UseCases.Queries.ReporteVisitaGuiada;
using Core.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Controllers.VisitasGrupales
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReportesVisitasGrupalesController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus;
        // REPORTE SUMMARY CARDS

        [HttpGet("reportSummaryCards")]
        public async Task<IActionResult> ReporteVisitasGrupales (

        [FromQuery] DateTime desde, 
        [FromQuery] DateTime hasta)
        {
            var reporteGeneral = await _commandQueryBus.Send(
                new ReporteGeneralVisitaGrupalQuery

                {
                    Desde = desde,
                    Hasta = hasta,

                });

            return Ok(reporteGeneral);
        }


        // ======================================================
        // VISITAS GUIADAS
        // ======================================================

        [HttpGet("reporteVisitaGuiada")]
        public async Task<IActionResult> ReporteVisitasGuiadas(

        [FromQuery] DateTime desde,
        [FromQuery] DateTime hasta)
        {
            var reporte = await _commandQueryBus.Send(
                new ReporteVisitaGuiadaQuery

                {
                    Desde = desde,
                    Hasta = hasta,

                });

            return Ok(reporte);
        }
        // ======================================================
        // VISITAS AUTOGUIADAS
        // ======================================================
        [HttpGet("reporteVisitaAutoguiada")]
        public async Task<IActionResult> ReporteVisitasAutoguiadas(

        [FromQuery] DateTime desde,
        [FromQuery] DateTime hasta)
        {
            var reporte = await _commandQueryBus.Send(
                new ReporteVisitaAutoguiadaQuery

                {
                    Desde = desde,
                    Hasta = hasta,

                });

            return Ok(reporte);
        }
        [HttpGet("reportGrid")]
        public async Task<IActionResult> TablaVisitasGrupales(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta
            )
        {
            var reporte = await _commandQueryBus.Send(
                new ReporteVisitasGrupalesQuery
                {
                    Desde = desde,
                    Hasta = hasta,
                   
                });

            return Ok(reporte);
        }

        [HttpGet("reporteVisitasSala")]
        public async Task<IActionResult> CantidadVisitasSala(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta
            )
        {
            var reporte = await _commandQueryBus.Send(
                new ReporteVisitanteSalaQuery
                {
                    FechaInicio = desde,
                    FechaFin = hasta,
                });

            return Ok(reporte);
        }



    }
}
