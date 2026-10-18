namespace Application.VisitaGrupal.DataTransferObjets.Guias
{
    public class AusenciaGuiaDto
    {
        public string GuiaId { get; private set; } = string.Empty;
        public string NombreGuia { get; private set; } = string.Empty;
        public string ApellidoGuia { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Telefono { get; private set; } = string.Empty;
        public DateTime FechaDesde { get; private set; }
        public DateTime FechaHasta { get; private set; }
        public string Motivo { get; private set; } = string.Empty;

        public AusenciaGuiaDto()
        {
        }

        public AusenciaGuiaDto(
            string guiaId,
            string nombreGuia,
            string apellidoGuia,
            string email,
            string telefono,
            DateTime fechaDesde,
            DateTime fechaHasta,
            string motivo)
        {
            GuiaId = guiaId;
            NombreGuia = nombreGuia;
            ApellidoGuia = apellidoGuia;
            Email = email;
            Telefono = telefono;
            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
            Motivo = motivo;
        }
    }
}