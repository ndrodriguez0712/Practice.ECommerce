using Identity.Domain;
using Identity.Service.EventHandlers.Commands;
using Identity.Service.EventHandlers.Helpers.Interfaces;
using Identity.Service.EventHandlers.Responses;
using MediatR;

namespace Identity.Service.EventHandlers
{
    public class UserCreateEventHandler : IRequestHandler<UserCreateCommand, IdentityResult>
    {
        #region Variables
        private readonly IUserAuthManager _userAuthManager;
        #endregion

        #region Constructor
        public UserCreateEventHandler(IUserAuthManager userAuthManager)
        {
            _userAuthManager = userAuthManager;
        }
        #endregion

        public async Task<IdentityResult> Handle(UserCreateCommand notification, CancellationToken cancellationToken)
        {
            var result = new IdentityResult();

            var user = new ApplicationUser
            {
                FirstName = notification.FirstName,
                LastName = notification.LastName,
                Email = notification.Email                
            };

            return result;
        }
    }
}
