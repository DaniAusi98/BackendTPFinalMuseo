namespace Application.PersonalMuseo.ApplicationServices
{
    public interface IConfirmChangeEmailUrlDashboard
    {
        string GetChangeEmailConfirmationUrl(
            string userId,
            string nuevoEmail,
            string token);
    }
}