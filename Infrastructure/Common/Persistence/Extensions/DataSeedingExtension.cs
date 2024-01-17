using Domain.Entities;
using Infrastructure.Common.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Persistence.Extensions;

public static class DataSeedingExtension
{
    public static void SeedTables(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(CitySeeding.SeedData());
        modelBuilder.Entity<Guest>().HasData(GuestSeeding.SeedData());
        modelBuilder.Entity<Owner>().HasData(OwnerSeeding.SeedData());
        modelBuilder.Entity<Hotel>().HasData(HotelSeeding.SeedData());
        modelBuilder.Entity<RoomType>().HasData(RoomTypeSeeding.SeedData());
        modelBuilder.Entity<Room>().HasData(RoomSeeding.SeedData());
        modelBuilder.Entity<Booking>().HasData(BookingSeeding.SeedData());
        modelBuilder.Entity<Payment>().HasData(PaymentSeeding.SeedData());
        modelBuilder.Entity<Review>().HasData(ReviewSeeding.SeedData());
    }
}