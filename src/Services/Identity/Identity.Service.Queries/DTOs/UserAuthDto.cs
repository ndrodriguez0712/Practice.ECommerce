namespace Identity.Service.Queries.DTOs
{
    public class UserAuthDto : UserDto
    {
        public string TokenCaptcha { get; set; }
    }
}
