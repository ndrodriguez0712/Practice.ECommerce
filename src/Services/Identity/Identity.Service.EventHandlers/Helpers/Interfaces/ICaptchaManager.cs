namespace Identity.Service.EventHandlers.Helpers.Interfaces
{
    public interface ICaptchaManager
    {
        Task<bool> IsValid(string token);
    }
}