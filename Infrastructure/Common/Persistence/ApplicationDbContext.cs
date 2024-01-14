using System.Reflection;
using Domain.Entities;
using Infrastructure.Common.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Persistence;

public class ApplicationDbContext : DbContext
{
    // public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    // {
    //     
    // }

    public DbSet<City> Cities { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=DESKTOP-EKG9OL3\SQLEXPRESS;Database=TravelAndAccommodationBooking;Trusted_Connection=True;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.SeedTables();
        base.OnModelCreating(modelBuilder);
    }
}