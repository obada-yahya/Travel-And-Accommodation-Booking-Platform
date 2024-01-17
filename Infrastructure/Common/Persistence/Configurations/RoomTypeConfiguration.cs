using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Common.Persistence.Configurations;

public class RoomTypeConfiguration: IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder
            .HasIndex(roomType => roomType.Category)
            .IsUnique();

        builder
            .Property(roomType => roomType.Category)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(roomType => roomType.PricePerNight)
            .IsRequired();
        
        builder.ToTable(roomType =>
            roomType
            .HasCheckConstraint
            ("CK_RoomType_PriceRange", "[PricePerNight] >= 0"));
    }
}