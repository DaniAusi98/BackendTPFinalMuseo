namespace Application.VisitaGrupal.DataTransferObjets
{
    public class HorarioDisponibleDto
    {
        public DayOfWeek Dia { get; set; }

        public TimeOnly Desde { get; set; }

        public TimeOnly Hasta { get; set; }
    }
}