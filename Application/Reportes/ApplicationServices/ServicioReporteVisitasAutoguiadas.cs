using Application.Reportes.DataTransferObjets;
using Domain.Reportes.Entities;
using Domain.VisitasGrupales.Entities;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Application.Reportes.ApplicationServices
{
    public class ServicioReporteVisitasAutoguiadas: IServicioReporteVisitasAutoguiadas
    {
        public ReporteVisitaAutoguiadaDto GenerarReporteVisitasAutoguiadas(
          IReadOnlyCollection<VisitaGrupalAutoguiada> visitasGrupalesAutoguiadas,int capacidadTotalDisponible)
        {
            int reservasTotales = visitasGrupalesAutoguiadas.Count;

            int cantidadVisitantesAutoguiadas = 0;

            int visitasAutoguiadasConfirmadas = 0;
            int visitasAutoguiadasCanceladas = 0;
            int visitasAutoguiadasPendientes = 0;
            int visitasAutoguiadasReprogramadas = 0;

            foreach (var visita in visitasGrupalesAutoguiadas)
            {
                cantidadVisitantesAutoguiadas += (int)visita.CantidadPersonas;
                if (visita.Estado == EstadoActividad.Activa)
                    visitasAutoguiadasConfirmadas++;

                if (visita.Estado == EstadoActividad.Cancelada)
                    visitasAutoguiadasCanceladas++;

                if (visita.EstadoConfirmacion == EstadoConfirmacionVisita.PendienteConfirmar)
                    visitasAutoguiadasPendientes++;

                if (visita.Estado == EstadoActividad.Reprogramada)
                    visitasAutoguiadasReprogramadas++;
            }



            int cantidadVisitantesTotales = cantidadVisitantesAutoguiadas;
            decimal tasaOcupacion = capacidadTotalDisponible == 0 ? 0 : (decimal)cantidadVisitantesTotales / capacidadTotalDisponible * 100;


            return new ReporteVisitaAutoguiadaDto(
                reservasTotales,
                cantidadVisitantesTotales,
                visitasAutoguiadasConfirmadas,
                visitasAutoguiadasCanceladas,
                visitasAutoguiadasReprogramadas,
                visitasAutoguiadasPendientes,
                tasaOcupacion
            );
        }
    }
}
