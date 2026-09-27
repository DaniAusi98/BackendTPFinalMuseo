

using Core.Domain.Entities;

namespace Domain.Reportes.Entities
{
    public class ReporteVisitasGuiadas : DomainEntity<int>
    {

        public int ReservasTotales { get; private set; }
        public int VisitanteTotales { get; private set; }
        public int VisitasConfirmadas { get; private set; }
        public int VisitasCanceladas { get; private set; }
        public int Reprogramadas { get; private set; }
        public int Pendientes { get; private set; }
        public decimal TasaOcupacion { get; private set; } = 0;


        public ReporteVisitasGuiadas(
            int reservasTotales,
            int visitanteTotales,
            int visitasConfirmadas,
            int visitasCanceladas,
            int reprogramadas,
            int pendientes,
            decimal tasaOcupacion 
            )
        {
            if (reservasTotales < 0)
                throw new ArgumentException("El número de reservas totales no puede ser negativo.", nameof(reservasTotales));
            if (visitanteTotales < 0)
                throw new ArgumentException("El número de visitantes totales no puede ser negativo.", nameof(visitanteTotales));
            if (visitasConfirmadas < 0)
                throw new ArgumentException("El número de visitas confirmadas no puede ser negativo.", nameof(visitasConfirmadas));
            if (visitasCanceladas < 0)
                throw new ArgumentException("El número de visitas canceladas no puede ser negativo.", nameof(visitasCanceladas));
            if (reprogramadas < 0)
                throw new ArgumentException("El número de visitas reprogramadas no puede ser negativo.", nameof(reprogramadas));
            if (pendientes < 0)
                throw new ArgumentException("El número de pendientes no puede ser negativo.", nameof(pendientes));
            if (tasaOcupacion < 0)
                throw new ArgumentException("La tasa de ocupación no puede ser negativa.", nameof(tasaOcupacion));


            ReservasTotales = reservasTotales;
            VisitanteTotales = visitanteTotales;
            VisitasConfirmadas = visitasConfirmadas;
            VisitasCanceladas = visitasCanceladas;
            Reprogramadas = reprogramadas;
            Pendientes = pendientes;
            TasaOcupacion = tasaOcupacion;

        }

        public void UpdateReporteVisitasGuiadas(
            int reservasTotales,
            int visitanteTotales,
            int visitasConfirmadas,
            int visitasCanceladas,
            int reprogramadas,
            int pendientes,
            decimal tasaOcupacion
            )
        {
            if (reservasTotales < 0)
                throw new ArgumentException("El número de reservas totales no puede ser negativo.", nameof(reservasTotales));
            if (visitanteTotales < 0)
                throw new ArgumentException("El número de visitantes totales no puede ser negativo.", nameof(visitanteTotales));
            if (visitasConfirmadas < 0)
                throw new ArgumentException("El número de visitas confirmadas no puede ser negativo.", nameof(visitasConfirmadas));
            if (visitasCanceladas < 0)
                throw new ArgumentException("El número de visitas canceladas no puede ser negativo.", nameof(visitasCanceladas));
            if (reprogramadas < 0)
                throw new ArgumentException("El número de visitas reprogramadas no puede ser negativo.", nameof(reprogramadas));
            if (pendientes < 0)
                throw new ArgumentException("El número de pendientes no puede ser negativo.", nameof(pendientes));
            if (tasaOcupacion < 0)
                throw new ArgumentException("La tasa de ocupación no puede ser negativa.", nameof(tasaOcupacion));
            ReservasTotales = reservasTotales;
            VisitanteTotales = visitanteTotales;
            VisitasConfirmadas = visitasConfirmadas;
            VisitasCanceladas = visitasCanceladas;
            Reprogramadas = reprogramadas;
            Pendientes = pendientes;
            TasaOcupacion = tasaOcupacion;


        }
    }
}