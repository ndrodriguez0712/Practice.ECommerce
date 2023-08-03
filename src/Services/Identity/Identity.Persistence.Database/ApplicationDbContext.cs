using Identity.Domain;
using Identity.Persistence.Database.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.ApplicationUser> Users { get; set; }
        public DbSet<ApplicationUserRole> Roles { get; set; }
        public DbSet<ApplicationUserStatus> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Database schema
            builder.HasDefaultSchema("User");

            // Model Contraints
            ModelConfig(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=PracticeECommerce;Integrated Security=true;Trust Server Certificate=true;");
        }

        #region Private Methods
        private void ModelConfig(ModelBuilder modelBuilder)
        {
            new ApplicationUserConfiguration(modelBuilder.Entity<Domain.ApplicationUser>());
            new ApplicationUserRoleConfiguration(modelBuilder.Entity<Domain.ApplicationUserRole>());
            new ApplicationUserStatusConfiguration(modelBuilder.Entity<Domain.ApplicationUserStatus>());
        }
        #endregion
    }
}
