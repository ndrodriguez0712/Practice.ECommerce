using Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistence.Database.Configuration
{
    public class ApplicationUserConfiguration
    {
        public ApplicationUserConfiguration(EntityTypeBuilder<ApplicationUser> entityBuilder)
        {
            entityBuilder.ToTable("User");

            entityBuilder.HasKey(x => x.Id);

            entityBuilder.HasIndex(e => e.Email, "UQ__Email__6B0F5AE070734E4D").IsUnique();

            entityBuilder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);

            entityBuilder.Property(x => x.LastName).IsRequired().HasMaxLength(100);

            entityBuilder.Property(x => x.Email).IsRequired().HasMaxLength(250).IsUnicode(false);

            entityBuilder.Property(x => x.Password).IsRequired().HasMaxLength(150);

            entityBuilder.Property(x => x.Phone).IsRequired().HasMaxLength(30).IsUnicode(false);

            entityBuilder.Property(x => x.Question).IsRequired().HasMaxLength(250);

            entityBuilder.Property(x => x.Answer).IsRequired().HasMaxLength(250);

            entityBuilder.Property(x => x.SignUpDate).HasColumnType("datetime");

            entityBuilder.Property(x => x.LastLoginDate).HasColumnType("datetime");

            entityBuilder.Property(e => e.EmailVerification).IsRequired();

            entityBuilder.HasOne(d => d.Status).WithMany(u => u.Users).HasForeignKey(d => d.IdStatus).OnDelete(DeleteBehavior.ClientSetNull);

            var userAdmin = new ApplicationUser
            { 
                FirstName = "Nicolás D.",
                LastName = "Rodríguez",
                Email = "nico.d.rodriguez@hotmail.com",
                Password = "ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f",
                Phone = "1122223333",
                Question = "asd",
                Answer = "asd",
                SignUpDate = DateTime.Now,
                IdStatus = 1,
                EmailVerification = true
            };

            entityBuilder.HasData(userAdmin);
        }
    }
}
