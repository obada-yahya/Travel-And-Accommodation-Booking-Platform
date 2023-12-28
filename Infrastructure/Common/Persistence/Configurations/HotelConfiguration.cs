using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Common.Persistence.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder
            .Property(hotel => hotel.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(hotel => hotel.Rating)
            .IsRequired()
            .HasDefaultValue(0.0F);

        builder
            .Property(hotel => hotel.StreetAddress)
            .IsRequired()
            .HasMaxLength(50);
        
        builder
            .Property(hotel => hotel.Description)
            .IsRequired()
            .HasMaxLength(100);
        
        builder
            .Property(hotel => hotel.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15);

        builder
            .Property(hotel => hotel.FloorsNumber)
            .IsRequired();
        
        builder.ToTable(hotel =>
            hotel
            .HasCheckConstraint
            ("CK_Hotel_RatingRange", "[Rating] >= 0 AND [Rating] <= 5"));
    }
}
