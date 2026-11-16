using Core.Application;

public class UpdatePersonalMuseoCommand : IRequestCommand
{
    public string UserId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string RolId { get; set; } = string.Empty;
}