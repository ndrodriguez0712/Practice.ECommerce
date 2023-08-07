using Identity.Service.EventHandlers.Commands;
using Identity.Service.EventHandlers.Helpers;
using Identity.Service.EventHandlers.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Identity.Service.EventHandlers
{
    public class UserLoginEventHandler :
        IRequestHandler<UserLoginCommand, IdentityAccess>
    {
        #region Variables
        private readonly SignInManager _signInManager;
        #endregion

        #region Constructor
        public UserLoginEventHandler(SignInManager signInManager)
        {
            _signInManager = signInManager;
        }
        #endregion

        public async Task<IdentityAccess> Handle(UserLoginCommand notification, CancellationToken cancellationToken)
        {
            var result = new IdentityAccess();

            var user = await _context.Users.SingleAsync(x => x.Email == notification.Email);
            var response = await _signInManager.CheckPasswordSignInAsync(user, notification.Password, false);

            if (response.Succeeded)
            {
                result.Succeeded = true;
                await GenerateToken(user, result);
            }

            return result;
        }

        private async Task GenerateToken(ApplicationUser user, IdentityAccess identity)
        {
            var secretKey = _configuration.GetValue<string>("SecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            var roles = await _context.Roles
                                      .Where(x => x.UserRoles.Any(y => y.UserId == user.Id))
                                      .ToListAsync();

            foreach (var role in roles)
            {
                claims.Add(
                    new Claim(ClaimTypes.Role, role.Name)
                );
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            identity.AccessToken = tokenHandler.WriteToken(createdToken);
        }
    }
}
