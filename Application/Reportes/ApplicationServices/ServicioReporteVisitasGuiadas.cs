using Application.Reportes.DataTransferObjets;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Enums.Enums;


namespace Application.Reportes.ApplicationServices
{
    public class ServicioReporteVisitasGuiadas:IServicioReporteVisitasGuiadas
    {
        public  ReporteVisitaGuiadaDto GenerarReporteGeneralVisitasGuiadas(
           IReadOnlyCollection<VisitaGrupalGuiada> visitasGrupalesGuiadas,int capacidadTotalDisponible)
        {
            int reservasTotales = visitasGrupalesGuiadas.Count;

            int cantidadVisitantesGuiadas = 0;

            int visitasGrupalesConfirmadas = 0;
            int visitasGrupalesCanceladas = 0;
            int visitasGrupalesPendientes = 0;
            int visitasGrupalesReprogramadas = 0;

            foreach (var visita in visitasGrupalesGuiadas)
            {
                cantidadVisitantesGuiadas += (int)visita.CantidadPersonas;

                if (visita.Estado == EstadoActividad.Activa)
                    visitasGrupalesConfirmadas++;

                if (visita.Estado == EstadoActividad.Cancelada)
                    visitasGrupalesCanceladas++;

                if (visita.EstadoConfirmacion == EstadoConfirmacionVisita.PendienteConfirmar)
                    visitasGrupalesPendientes++;

                if (visita.Estado == EstadoActividad.Reprogramada)
                    visitasGrupalesReprogramadas++;
            }

           

            int cantidadVisitantesTotales = cantidadVisitantesGuiadas;
            decimal tasaOcupacion = capacidadTotalDisponible == 0 ? 0 : (decimal)cantidadVisitantesTotales / capacidadTotalDisponible * 100;

            return new ReporteVisitaGuiadaDto(
                reservasTotales,
                cantidadVisitantesTotales,
                visitasGrupalesConfirmadas,
                visitasGrupalesCanceladas,
                visitasGrupalesReprogramadas,
                visitasGrupalesPendientes,
                tasaOcupacion

            );
        }
    }
}
