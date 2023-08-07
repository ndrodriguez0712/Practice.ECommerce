using Identity.Domain;

namespace Identity.Service.EventHandlers.Helpers.Interfaces
{
    public interface ISignInManager
    {        
        Task<ApplicationUser> SingleAsync(string email);
        Task<bool> CheckPasswordSignInAsync(string user, string password);
    }
}