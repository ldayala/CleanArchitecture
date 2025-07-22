
using CleanArchitecture.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
          
            builder.ToTable("roles");
                builder.HasKey(r => r.Id);

                builder.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(50);

            // Seed initial roles
            builder.HasData(Role.GetValues());

            builder.HasMany(r => r.Permissions)
                .WithMany()
                .UsingEntity<RolePermission>();


        }
    }
}
