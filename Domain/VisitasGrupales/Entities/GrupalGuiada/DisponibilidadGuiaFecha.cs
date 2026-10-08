namespace Domain.VisitasGrupales.Entities.GrupalGuiada
{
    public class DisponibilidadGuiaFecha
    {
        public DateOnly Fecha { get; private set; }
        public TimeOnly HoraInicio { get; private set; }
        public TimeOnly HoraFin { get; private set; }
        public bool Disponible { get; private set; }

        public DisponibilidadGuiaFecha(DateOnly fecha, TimeOnly horaInicio, TimeOnly horaFin, bool disponible)
        {

            if (horaFin <= horaInicio)
                throw new ArgumentException("La hora de fin debe ser mayor que la hora de inicio.");
            Fecha = fecha;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
            Disponible = disponible;
        }
    }
}
