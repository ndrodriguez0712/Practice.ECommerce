using Identity.Service.EventHandlers.Helpers.Interfaces;
using Identity.Service.Queries.DTOs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Identity.Service.EventHandlers.Helpers
{
    public class CaptchaManager : ICaptchaManager
    {
        #region Variable
        private readonly CaptchaConfigurationDto _captchaConfiguration;
        #endregion

        #region Constructor
        public CaptchaManager(IOptions<CaptchaConfigurationDto> captchaConfiguration)
        {
            _captchaConfiguration = captchaConfiguration.Value;
        }
        #endregion

        public async Task<bool> IsValid(string token)
        {
            if (_captchaConfiguration.Activated != false)
            {
                var googleVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";
                using var client = new HttpClient();
                try
                {
                    var postTask = await client.PostAsync($"{googleVerificationUrl}?secret={_captchaConfiguration.SecretKey}&response={token}", null);
                    var result = await postTask.Content.ReadAsStringAsync();
                    var resultObject = JObject.Parse(result);
                    var success = resultObject["success"];
                    var response = (bool)success;
                    if (!response)
                    {
                        throw new Exception("Invalid captcha, please try again.");
                    }
                    return response;

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
            }

            return false;
        }
    }
}
