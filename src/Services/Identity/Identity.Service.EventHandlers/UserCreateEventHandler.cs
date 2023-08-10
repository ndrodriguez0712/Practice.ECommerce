using Identity.Domain;
using Identity.Service.EventHandlers.Commands;
using Identity.Service.EventHandlers.Helpers.Interfaces;
using Identity.Service.EventHandlers.Responses;
using MediatR;
using static Identity.Common.Enums;
using Service.Common.Mapping;

namespace Identity.Service.EventHandlers
{
    public class UserCreateEventHandler : IRequestHandler<UserCreateCommand, IdentityResult>
    {
        #region Variables
        private readonly IUserAuthManager _userAuthManager;
        private readonly IEncryptionManager _encryptionManager;
        private readonly ICaptchaManager _captchaManager;
        private readonly IEmailSenderManager _emailSenderManager;
        #endregion

        #region Constructor
        public UserCreateEventHandler(IUserAuthManager userAuthManager, IEncryptionManager encryptionManager, ICaptchaManager captchaManager, IEmailSenderManager emailSenderManager)
        {
            _userAuthManager = userAuthManager;
            _encryptionManager = encryptionManager;
            _captchaManager = captchaManager;
            _emailSenderManager = emailSenderManager;            
        }
        #endregion

        public async Task<IdentityResult> Handle(UserCreateCommand notification, CancellationToken cancellationToken)
        {
            var result = new IdentityResult();

            try
            {
                await _captchaManager.IsValid(notification.TokenCaptcha);

                var userDb = await _userAuthManager.GetUserAsync(notification.Email);

                if (userDb != null)
                    return result;

                var user = notification.MapTo<ApplicationUser>();

                user.Password = _encryptionManager.Hash(user.Password);
                user.IdStatus = (int)UserStatusEnum.Pending;
                user.SignUpDate = DateTime.Now;
                user.LastLoginDate = DateTime.Now;
                user.IdRole = (int)UserRoleEnum.Visit;
                user.EmailVerification = false;

                await _userAuthManager.CreateUserAsync(user);
                await _emailSenderManager.SendUserCreatedEmailAsync(user.Email, $"{user.FirstName} {user.LastName}");

                result.Succeeded = true;

                return result;
            }
            catch (Exception ex) 
            {
                result.Succeeded = false;
                foreach(var error in ex.Data)
                {
                    result.Errors.Add(new IdentityError {
                                                            Code = error.ToString() ?? "Code Unknown" ,
                                                            Description = ex.InnerException != null ? ex.InnerException.Message : ex.Message
                                                        });
                }

                return result;
            }
        }
    }
}
