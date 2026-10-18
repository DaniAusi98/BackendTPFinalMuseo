namespace Application.VisitaGrupal.DataTransferObjets.Guias
{
    public class GuiaDto
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public bool Activo { get; set; }

        public List<GuiaHorarioDto> Horarios { get; set; } = new();
    }
}