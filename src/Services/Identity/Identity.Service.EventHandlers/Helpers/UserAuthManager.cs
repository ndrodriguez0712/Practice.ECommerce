using Identity.Domain;
using Identity.Persistence.Database;
using Identity.Persistence.Database.Interfaces;
using Identity.Service.EventHandlers.Helpers.Interfaces;
using Identity.Service.EventHandlers.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Service.EventHandlers.Helpers
{
    public class UserAuthManager : IUserAuthManager
    {
        #region Variables
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructor        
        public UserAuthManager(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;            
        }
        #endregion

        //public async Task<ApplicationUser> CreateUserAsync()
        //{

        //}

        public async Task<ApplicationUser> GetUserAsync(string email)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IUnitOfWork<ApplicationDbContext> _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
            IBaseRepository<ApplicationUser> _applicationUserRepository = _unitOfWork.GetRepository<ApplicationUser>();

            return await _applicationUserRepository.FirstOrDefaultAsync(x => x.Email == email);
        }

        public bool CheckPasswordSignIn(string passwordHashed, string password)
        {
            string hashed;

            using (SHA256 mySHA256 = SHA256.Create())
            {
                SHA256 sha256 = SHA256.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                StringBuilder sb = new StringBuilder();
                stream = sha256.ComputeHash(encoding.GetBytes(password));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                hashed = sb.ToString();
            }

            if (passwordHashed == hashed)
                return true;
            else
                return false;
        }

        public string Hash(string password)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                SHA256 sha256 = SHA256.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                StringBuilder sb = new StringBuilder();
                stream = sha256.ComputeHash(encoding.GetBytes(password));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                var hashed = sb.ToString();
                return hashed;
            }
        }

        public IdentityAccess GenerateToken(ApplicationUser user, string rol)
        {
            // Header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtIssuerOptions:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, $"{user.FirstName}_{user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("IdUser", user.Id.ToString())
            };

            new Claim(ClaimTypes.Role, rol);

            var expiration = DateTime.UtcNow.AddDays(3);

            //Payload
            var payload = new JwtPayload(
                _configuration["JwtIssuerOptions:Issuer"],
                _configuration["JwtIssuerOptions:Audience"],
                claims,
                DateTime.Now,
                expiration);

            var jwtSecurity = new JwtSecurityToken(header, payload);

            return new IdentityAccess()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurity),
                ExpirationDate = expiration,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Succeeded = true
            };
        }
    }
}
