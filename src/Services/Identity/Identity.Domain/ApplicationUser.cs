namespace Identity.Domain
{
    public class ApplicationUser
    {
        public int Id { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }        
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime? SignUpDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int IdRole { get; set; }
        public int IdStatus { get; set; }
        public bool EmailVerification { get; set; }
        public virtual ApplicationUserRole Role { get; set; }
        public virtual ApplicationUserStatus Status { get; set; }
    }
}
