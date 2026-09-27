using Application.Reportes.DataTransferObjets;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Enums.Enums;


namespace Application.Reportes.ApplicationServices
{
    public class ServicioReporteVisitasGrupales:IServicioReporteVisitasGrupales
    {
        public ReporteVisitasGrupalesDto GenerarReporteGeneralVisitasGrupales(
            IReadOnlyCollection<VisitaGrupalAutoguiada> visitaGrupalAutoguiadas,
            IReadOnlyCollection<VisitaGrupalGuiada> visitaGrupalGuiadas)
        {
            int reservasTotales =
                visitaGrupalAutoguiadas.Count + visitaGrupalGuiadas.Count;

            int cantidadVisitantesGuiadas = 0;
            int cantidadVisitantesAutoguiadas = 0;

            int visitasGrupalesConfirmadas = 0;
            int visitasGrupalesCanceladas = 0;
            int visitasGrupalesPendientes = 0;
            int visitasGrupalesReprogramadas = 0;

            foreach (var visita in visitaGrupalAutoguiadas)
            {
                cantidadVisitantesAutoguiadas += (int)visita.CantidadPersonas;

                if (visita.Estado == EstadoActividad.Activa)
                    visitasGrupalesConfirmadas++;

                if (visita.Estado == EstadoActividad.Cancelada)
                    visitasGrupalesCanceladas++;

                if (visita.EstadoConfirmacion == EstadoConfirmacionVisita.PendienteConfirmar)
                    visitasGrupalesPendientes++;

                if (visita.Estado == EstadoActividad.Reprogramada)
                    visitasGrupalesReprogramadas++;
            }

            foreach (var visita in visitaGrupalGuiadas)
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

            int cantidadVisitantesTotales =
                cantidadVisitantesAutoguiadas + cantidadVisitantesGuiadas;

            return new ReporteVisitasGrupalesDto(
                reservasTotales,
                cantidadVisitantesTotales,
                visitasGrupalesConfirmadas,
                visitasGrupalesCanceladas,
                visitasGrupalesReprogramadas,
                visitasGrupalesPendientes
            );
        }
    }
}
