namespace Application.ConfiguracionSistema.DataTransferObjets
{
    public class HorarioAperturaDto
    {
        public DayOfWeek Dia { get; set; }

        public TimeOnly Desde { get; set; }

        public TimeOnly Hasta { get; set; }
    }
}
