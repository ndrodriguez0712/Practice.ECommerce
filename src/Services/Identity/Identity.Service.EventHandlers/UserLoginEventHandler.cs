using Identity.Service.EventHandlers.Commands;
using Identity.Service.EventHandlers.Helpers.Interfaces;
using Identity.Service.EventHandlers.Responses;
using MediatR;

namespace Identity.Service.EventHandlers
{
    public class UserLoginEventHandler : IRequestHandler<UserLoginCommand, IdentityAccess>
    {
        #region Variables
        private readonly IUserAuthManager _userAuthManager;
        #endregion

        #region Constructor
        public UserLoginEventHandler(IUserAuthManager userAuthManager)
        {
            _userAuthManager = userAuthManager;
        }
        #endregion

        public async Task<IdentityAccess> Handle(UserLoginCommand notification, CancellationToken cancellationToken)
        {
            var result = new IdentityAccess();

            var user = await _userAuthManager.GetUserAsync(notification.Email);

            if(user == null)
                return result;

            var response = _userAuthManager.CheckPasswordSignIn(user.Password, notification.Password);

            if (response)
                return _userAuthManager.GenerateToken(user, user.Role.Name);

            return result;
        }        
    }
}
