namespace Application.Usuario.ApplicationServices.ApplicationServiceInterfaces
{
    public interface IConfirmUserUrl
    {
        string GetEmailConfirmationUrl(
            string userId,
            string token);
    }
}
