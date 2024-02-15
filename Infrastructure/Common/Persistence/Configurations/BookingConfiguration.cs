using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Common.Persistence.Configurations;

public class BookingConfiguration: IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder
            .HasOne<Room>()
            .WithMany()
            .IsRequired()
            .HasForeignKey(booking => booking.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(booking => booking.CheckInDate)
            .IsRequired();
        
        builder
            .Property(booking => booking.CheckOutDate)
            .IsRequired();

        builder
            .Property(booking => booking.Price)
            .IsRequired();
    }
}