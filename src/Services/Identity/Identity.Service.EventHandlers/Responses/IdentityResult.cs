namespace Identity.Service.EventHandlers.Responses
{
    public class IdentityResult
    {
        public bool Succeeded { get; set; } = false;
        public List<IdentityError> Errors { get; set; } = new List<IdentityError>();
    }
}
