using Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistence.Database.Configuration
{
    public class ApplicationUserRoleConfiguration
    {
        public ApplicationUserRoleConfiguration(EntityTypeBuilder<ApplicationUserRole> entityBuilder)
        {
            entityBuilder.ToTable("UserRole");

            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Name).IsRequired();

            entityBuilder.Property(x => x.Description).IsRequired();

            var userRoles = new List<ApplicationUserRole>();

            var adminRole = new ApplicationUserRole
            {
                Id = 1,
                Name = "Administrator",
                Description = "Full access"
            };

            userRoles.Add(adminRole);

            var basicRole = new ApplicationUserRole
            {
                Id = 2,
                Name = "Basic",
                Description = "Basic operations access"
            };

            userRoles.Add(basicRole);

            var visitRole = new ApplicationUserRole
            {
                Id = 3,
                Name = "Visit",
                Description = "Limited access"
            };

            userRoles.Add(visitRole);

            entityBuilder.HasData(userRoles);
        }
    }
}
