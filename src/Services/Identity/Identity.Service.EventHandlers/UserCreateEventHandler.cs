using Identity.Domain;
using Identity.Service.EventHandlers.Commands;
using Identity.Service.EventHandlers.Helpers.Interfaces;
using Identity.Service.EventHandlers.Responses;
using Identity.Service.Queries.DTOs;
using MediatR;
using static Identity.Common.Enums;
using System.Web;
using Service.Common.Mapping;

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

            var user = notification.MapTo<ApplicationUser>();
            user.Password = _userAuthManager.Hash(user.Password);

            //await _captchaServicio.IsValid(user.TokenCaptcha);

            var userDb = await _userAuthManager.GetUserAsync(user.Email);

            if (userDb != null)
                return result;

            user.IdStatus = (int)UserStatusEnum.Pending;
            user.SignUpDate = DateTime.Now;
            user.LastLoginDate = DateTime.Now;
            user.IdRole = (int)UserRoleEnum.Visit;
            user.EmailVerification = false;

            //await _usuarioGestor.AgregarUsuario(user);

            var encryptedText = _encriptacionServicio.Encriptar(user.Email.Trim(), _configuration["KeyEncriptacion"]);
            var encodedEncryptedText = HttpUtility.UrlEncode(encryptedText);

            var email = new EmailSolicitud()
            {
                Email = userDto.Email,
                NombreDestinatario = usuarioDto.NombreCompleto,
                Titulo = "Confirmación de cuenta Cedeira Servicios Factura Electrónica",
                Cuerpo = $"{_configuration["UrlEmailVerification"]}{encodedEncryptedText}",
                NombreDePlantilla = "VerificacionEmail.html"
            };

            _emailServicio.MandarEmailAsync(email);

            return true;

            return result;
        }
    }
}
