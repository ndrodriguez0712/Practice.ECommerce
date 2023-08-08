using Identity.Service.Queries.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using System.Net.Mail;

namespace Identity.Service.EventHandlers.Helpers
{
    public class EmailSenderManager
    {
        #region Variables
        private readonly SmtpConfigurationDto _smtpConfigurationDto;
        private readonly IConfiguration _configuracion;
        #endregion

        #region Constructor 
        public EmailSenderManager(IOptions<SmtpConfigurationDto> smtpConfigurationDto, IConfiguration configuration)
        {
            _smtpConfigurationDto = smtpConfigurationDto.Value;
            _configuracion = configuration;
        }
        #endregion

        public async Task MandarEmailAsync(EmailRequestDto emailRequest)
        {
            var message = new MimeMessage();

            if (emailRequest.SchemeName != null)
            {
                var builder = new BodyBuilder();
                var htmlBody = string.Empty;
                var schemeRootPath = ObtenerRutaRaiz("Email:SchemePath");
                var logoRootPath = ObtenerRutaRaiz("Email:PracticeECommerce");
                var urlPracticeECommerce = _configuracion.GetSection("Email:UrlPracticeECommerceFront").Value;
                var logoPracticeECommerce = ObtenerArchivo(logoRootPath, "LogoPracticeECommerce.png");
                var image = builder.LinkedResources.Add(logoPracticeECommerce);
                image.ContentId = MimeUtils.GenerateMessageId();

                using (StreamReader sr = System.IO.File.OpenText(ObtenerArchivo(schemeRootPath, emailRequest.SchemeName)))
                {
                    htmlBody = sr.ReadToEnd();
                }

                /*
                    {0} = Titulo del mail.
                    {1} = Logo de PracticeECommerce.
                    {2} = Nombre del destinatario.
                    {3} = Mensaje a envíar.
                    {4} = Url del front.
                */

                builder.HtmlBody = string.Format(htmlBody, emailRequest.Title, image.ContentId, emailRequest.DestinyName, emailRequest.Body, urlPracticeECommerce);

                message.Body = builder.ToMessageBody();
            }
            else
            {
                message.Body = new TextPart("html") { Text = emailRequest.Body };
            }

            message.From.Add(new MailboxAddress(_smtpConfigurationDto.UserName, _smtpConfigurationDto.From));
            message.To.Add(new MailboxAddress(emailRequest.DestinyName, emailRequest.Email));
            message.Subject = emailRequest.Title;

            using var client = new SmtpClient();
            client.CheckCertificateRevocation = false;
            await client.ConnectAsync(_smtpConfigurationDto.Servidor, _smtpConfigurationDto.Puerto, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpConfigurationDto.NombreUsuario, _smtpConfigurationDto.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        #region Metodos Privados
        private string ObtenerRutaRaiz(string section)
        {
            var rutaDefault = _configuration.GetSection(section).Value;
            var ruta = $"{AppDomain.CurrentDomain.BaseDirectory}{rutaDefault}";

#if DEBUG
            ruta = $"{Directory.GetParent("CedServicios.Api")}{rutaDefault}";
#endif

            if (!Directory.Exists(ruta))
                throw new Exception($"No se encontró la ruta de acceso a la ruta: {ruta}.");

            return ruta;
        }
        private string ObtenerArchivo(string ruta, string archivo)
        {
            var esquema = Directory.EnumerateFiles(ruta);

            if (esquema == null)
                throw new Exception($"La carpeta contenedora está vacia. Ruta de acceso: {ruta}.");

            var result = esquema.Where(x => x.Equals($"{ruta}{archivo}")).FirstOrDefault();

            if (result == null)
                throw new Exception($"No se encontró el objeto indicado, esquema: {archivo}.");

            return result;
        }
        #endregion
    }
}
