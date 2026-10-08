namespace Application.VisitaGrupal.DataTransferObjets
{
    public class DisponibilidadGuiaFechaDto
    {
        public DateOnly Fecha { get; private set; }
        public TimeOnly HoraInicio { get; private set; }
        public TimeOnly HoraFin { get; private set; }
        public bool Disponible { get; private set; }

    }
}
