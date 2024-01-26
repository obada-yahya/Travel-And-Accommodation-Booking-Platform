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
            .HasOne<User>() 
            .WithMany()
            .IsRequired()
            .HasForeignKey(booking => booking.GuestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}