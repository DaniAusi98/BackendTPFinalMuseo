namespace Application.Eventos.DataTransferObjets;

public class ArchivoNuevoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] Contenido { get; set; } = [];
}
