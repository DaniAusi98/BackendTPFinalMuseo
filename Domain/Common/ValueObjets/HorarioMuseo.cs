namespace Domain.Common.ValueObjets
{


    public sealed class HorarioApertura
    {
        public DayOfWeek Dia { get; private set; }
        public TimeOnly HoraInicio { get; private set; }
        public TimeOnly HoraFin { get; private set; }

        private HorarioApertura() { }



        public HorarioApertura(DayOfWeek dia, TimeOnly inicio, TimeOnly fin)
        {

            if (fin <= inicio)
                throw new ArgumentException(
                    "El horario de fin debe ser posterior al inicio");
            Dia = dia;
            HoraInicio = inicio;
            HoraFin = fin;
        }


    }

}
