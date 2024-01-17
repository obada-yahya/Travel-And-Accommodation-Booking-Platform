using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Common.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder
            .Property(appUser => appUser.PasswordHash)
            .IsRequired();
        
        builder
            .Property(appUser => appUser.Email)
            .IsRequired();
        
        builder
            .Property(appUser => appUser.Role)
            .IsRequired();
        
        builder
               .HasIndex(appUser => appUser.Email)
               .IsUnique();
    }
}