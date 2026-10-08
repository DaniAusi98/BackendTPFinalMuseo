namespace Application.PersonalMuseo.ApplicationServices
{
    public interface IConfirmUserUrlDashboard
    {
        string GetEmailConfirmationUserUrl(
            string userId,
            string token);
    }
}
