namespace Application.Eventos.ApplicationServices
{
    public interface IEnvioFormularioEventoEmailService
    {
        Task SendEnvioFormularioEmailAsync(string emailResponsable, string tituloPropuesta, string urlFormulario);

    }
}
