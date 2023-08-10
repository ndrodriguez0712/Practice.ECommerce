namespace Identity.Service.EventHandlers.Helpers.Interfaces
{
    public interface IEmailSenderManager
    {
        Task SendUserCreatedEmailAsync(string userEmail, string userCompleteName);
    }
}