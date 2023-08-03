using Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistence.Database.Configuration
{
    public class ApplicationUserStatusConfiguration
    {
        public ApplicationUserStatusConfiguration(EntityTypeBuilder<ApplicationUserStatus> entityBuilder)
        {
            entityBuilder.ToTable("UserStatus");

            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Name).IsRequired();

            entityBuilder.Property(x => x.Description).IsRequired();
        }
    }
}
