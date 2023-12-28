using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Common.Persistence.Configurations;

public class RoomTypeConfiguration: IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder
            .HasOne<Hotel>()
            .WithMany()
            .IsRequired()
            .HasForeignKey(roomType => roomType.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(roomType => roomType.Type)
            .IsUnique();

        builder
            .Property(roomType => roomType.Type)
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