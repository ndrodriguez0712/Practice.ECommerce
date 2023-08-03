namespace Identity.Domain
{
    public class ApplicationUserStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
