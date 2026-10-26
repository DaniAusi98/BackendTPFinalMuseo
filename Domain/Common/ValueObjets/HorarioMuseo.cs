namespace Domain.Common.ValueObjets
{


    public sealed class HorarioApertura
    {
        public DayOfWeek Dia { get; private set; }
        public TimeOnly Desde { get; private set; }
        public TimeOnly Hasta { get; private set; }

        private HorarioApertura() { }



        public HorarioApertura(DayOfWeek dia, TimeOnly inicio, TimeOnly fin)
        {

            if (fin <= inicio)
                throw new ArgumentException(
                    "El horario de fin debe ser posterior al inicio");
            Dia = dia;
            Desde = inicio;
            Hasta = fin;
        }


    }

}
