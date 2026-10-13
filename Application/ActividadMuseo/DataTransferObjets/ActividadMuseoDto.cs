namespace Application.ActividadMuseo.DataTransferObjets
{
    public class ActividadMuseoDto
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string TipoActividad { get; set; }
        public string Estado { get; set; }
        public int? CantidadPersonas { get; set; }
        public List<ActividadSalaDto> Salas { get; set; }
        public List<ActividadRecursoDto> Recursos { get; set; }
        public ActividadHorarioDto Horario { get; set; }

    }
}
