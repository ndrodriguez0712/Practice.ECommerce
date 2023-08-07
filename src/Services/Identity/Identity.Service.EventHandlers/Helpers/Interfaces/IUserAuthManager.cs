using Identity.Domain;
using Identity.Service.EventHandlers.Responses;

namespace Identity.Service.EventHandlers.Helpers.Interfaces
{
    public interface IUserAuthManager
    {
        Task<ApplicationUser> GetUserAsync(string email);
        bool CheckPasswordSignIn(string user, string password);
        string Hash(string password);
        IdentityAccess GenerateToken(ApplicationUser user, string rol);
    }
}