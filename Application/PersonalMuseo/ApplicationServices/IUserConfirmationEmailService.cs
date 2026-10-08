namespace Application.PersonalMuseo.ApplicationServices
{
    public interface IUserConfirmationEmailService
    {
        Task SendUserConfirmationEmailAsync(string userEmail, string confirmationUrl);

    }
}
