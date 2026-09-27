namespace Application.ApplicationMuseo.DataTransferObjects
{
    public class DepartamentoDto
    {
        public int Id { get; set; }
        public int ProvinciaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
