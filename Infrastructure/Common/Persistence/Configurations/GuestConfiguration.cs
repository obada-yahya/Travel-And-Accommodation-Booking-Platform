using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Common.Persistence.Configurations;

public class GuestConfiguration: IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder
            .Property(guest => guest.FirstName)
            .IsRequired()
            .HasMaxLength(25);

        builder
            .Property(guest => guest.LastName)
            .IsRequired()
            .HasMaxLength(25);

        builder
            .Property(guest => guest.Email)
            .IsRequired()
            .HasMaxLength(100);
        
        builder
            .Property(guest => guest.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15);
    }
}