using Identity.Service.EventHandlers.Helpers.Interfaces;
using Identity.Service.Queries.DTOs;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using System.Web;

namespace Identity.Service.EventHandlers.Helpers
{
    public class EmailSenderManager : IEmailSenderManager
    {
        #region Variables
        private readonly SmtpConfigurationDto _smtpConfigurationDto;
        private readonly IConfiguration _configuration;
        private readonly IEncryptionManager _encryptionService;
        #endregion

        #region Constructor 
        public EmailSenderManager(IOptions<SmtpConfigurationDto> smtpConfigurationDto, IConfiguration configuration, IEncryptionManager encryptionService)
        {
            _smtpConfigurationDto = smtpConfigurationDto.Value;
            _configuration = configuration;
            _encryptionService = encryptionService;
        }
        #endregion

        public async Task SendUserCreatedEmailAsync(string userEmail, string userCompleteName)
        {
            var encryptedText = _encryptionService.Encrypt(userEmail.Trim(), _configuration["EncryptionKey"]);
            var encodedEncryptedText = HttpUtility.UrlEncode(encryptedText);

            var email = new EmailRequestDto()
            {
                Email = userEmail,
                DestinyName = userCompleteName,
                Title = "Practice ECommerce account confirmation",
                Body = encodedEncryptedText,
                SchemeName = "VerificationEmail.html"
            };

            await MandarEmailAsync(email);
        }

        #region Metodos Privados
        private async Task MandarEmailAsync(EmailRequestDto emailRequest)
        {
            var message = new MimeMessage();
            var builder = new BodyBuilder();
            var htmlBody = string.Empty;
            var schemeRootPath = GetRootPath("Email:SchemePath");
            var logoRootPath = GetRootPath("Email:PracticeECommerce");
            var urlPracticeECommerce = _configuration.GetSection("Email:UrlPracticeECommerceFront").Value;
            var logoPracticeECommerce = GetFile(logoRootPath, "LogoPracticeECommerce.png");
            var image = builder.LinkedResources.Add(logoPracticeECommerce);
            image.ContentId = MimeUtils.GenerateMessageId();

            using (StreamReader sr = System.IO.File.OpenText(GetFile(schemeRootPath, emailRequest.SchemeName)))
            {
                htmlBody = sr.ReadToEnd();
            }

            /*
                {0} = Mail Title.
                {1} = PracticeECommerce Logo.
                {2} = Destiny mail.
                {3} = Message.
                {4} = Url of the Front.
            */

            builder.HtmlBody = string.Format(htmlBody, emailRequest.Title, image.ContentId, emailRequest.DestinyName, emailRequest.Body, urlPracticeECommerce);

            message.Body = builder.ToMessageBody();

            //-----------------------------------------------
            message.From.Add(new MailboxAddress(_smtpConfigurationDto.Email, _smtpConfigurationDto.From));
            message.To.Add(new MailboxAddress(emailRequest.DestinyName, emailRequest.Email));
            message.Subject = emailRequest.Title;

            using var client = new SmtpClient();
            client.CheckCertificateRevocation = false;
            await client.ConnectAsync(_smtpConfigurationDto.Server, _smtpConfigurationDto.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpConfigurationDto.Email, _smtpConfigurationDto.From);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        private string GetRootPath(string section)
        {
            var defaultPath = _configuration.GetSection(section).Value;
            var path = $"{AppDomain.CurrentDomain.BaseDirectory}{defaultPath}";

            #if DEBUG
            path = $"{Directory.GetParent("Identity.Api")}{defaultPath}";
            #endif

            if (!Directory.Exists(path))
                throw new Exception($"Path not found: {path}.");

            return path;
        }
        private string GetFile(string path, string file)
        {
            var scheme = Directory.EnumerateFiles(path);

            if (scheme == null)
                throw new Exception($"The containing folder is empty. Access path: {path}.");

            var result = scheme.Where(x => x.Equals($"{path}{file}")).FirstOrDefault();

            if (result == null)
                throw new Exception($"The indicated object was not found, schema: {file}.");

            return result;
        }
        #endregion
    }
}
