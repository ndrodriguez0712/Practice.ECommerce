namespace Identity.Service.Queries.DTOs
{
    public class CaptchaConfigurationDto
    {
        public string SiteKey { get; set; }
        public string SecretKey { get; set; }
        public string Vertion { get; set; }
        public bool Activated { get; set; }
    }
}
