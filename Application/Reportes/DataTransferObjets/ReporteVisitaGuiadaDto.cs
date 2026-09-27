namespace Application.Reportes.DataTransferObjets
{
    public class ReporteVisitaGuiadaDto
    {
        public int ReservasTotales { get; private set; }
        public int VisitanteTotales { get; private set; }
        public int VisitasConfirmadas { get; private set; }
        public int VisitasCanceladas { get; private set; }
        public int Reprogramadas { get; private set; }
        public int Pendientes { get; private set; }
        public decimal TasaOcupacion { get; private set; }

        public ReporteVisitaGuiadaDto(int reservasTotales, int visitanteTotales, int visitasConfirmadas, int visitasCanceladas, int reprogramadas, int pendientes, decimal tasaOcupacion)
        {
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
