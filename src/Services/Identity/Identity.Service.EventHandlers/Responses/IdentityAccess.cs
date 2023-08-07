namespace Identity.Service.EventHandlers.Responses
{
    public class IdentityAccess
    {
        public string? Token { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow;
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool Succeeded { get; set; }
    }
}
