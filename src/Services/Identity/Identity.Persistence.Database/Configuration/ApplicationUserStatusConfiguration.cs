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

            var userStatus = new List<ApplicationUserStatus>();

            var activeStatus = new ApplicationUserStatus
            {
                Id = 1,
                Name = "Active"
            };

            userStatus.Add(activeStatus);

            var inactiveStatus = new ApplicationUserStatus
            {
                Id = 2,
                Name = "Inactive"
            };

            userStatus.Add(inactiveStatus);

            var pendingStatus = new ApplicationUserStatus
            {
                Id = 3,
                Name = "Pending"
            };

            userStatus.Add(pendingStatus);

            var pendingSendStatus = new ApplicationUserStatus
            {
                Id = 4,
                Name = "PendingSend"
            };

            userStatus.Add(pendingSendStatus);

            var pendingConfirmationStatus = new ApplicationUserStatus
            {
                Id = 5,
                Name = "PendingConfirmation"
            };

            userStatus.Add(pendingConfirmationStatus);

            var rejectedStatus = new ApplicationUserStatus
            {
                Id = 6,
                Name = "Rejected"
            };

            userStatus.Add(rejectedStatus);

            entityBuilder.HasData(userStatus);
        }
    }
}
