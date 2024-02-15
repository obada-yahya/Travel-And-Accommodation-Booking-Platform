using Domain.Entities;

namespace Infrastructure.Common.Persistence.Seeding;

public class BookingSeeding
{
    public static IEnumerable<Booking> SeedData()
    {
        return new List<Booking>
        {
            new Booking
            {
                Id = new Guid("7d3155a2-95f8-4d9b-bc24-662ae053f1c9"),
                RoomId = new Guid("a98b8a9d-4c5a-4a90-a2d2-5f1441b93db6"),
                UserId = new Guid("c6c45f7c-2dfe-4c1e-9a9b-8b173c71b32c"),
                Price = 100,
                CheckInDate = DateTime.Parse("2023-01-15"),
                CheckOutDate = DateTime.Parse("2023-01-20"),
                BookingDate = DateTime.Parse("2023-01-10")
            },
            new Booking
            {
                Id = new Guid("efeb3d13-3dab-46c9-aa9a-9f22dd58e06e"),
                RoomId = new Guid("4e1cb3d9-bc3b-4997-a3d5-0c56cf17fe7a"),
                UserId = new Guid("aaf21a7d-8fc3-4c9f-8a8e-1eeec8dcd462"),
                Price = 150,
                CheckInDate = DateTime.Parse("2023-02-05"),
                CheckOutDate = DateTime.Parse("2023-02-10"),
                BookingDate = DateTime.Parse("2023-01-28")
            },
            new Booking
            {
                Id = new Guid("0bf4a177-98b8-4f67-8a56-95669c320890"),
                RoomId = new Guid("c6898b7e-ee09-4b36-8b20-22e8c2a63e29"),
                UserId = new Guid("f44c3eb4-2c8a-4a77-a31b-04c4619aa15a"),
                Price = 200,
                CheckInDate = DateTime.Parse("2023-03-12"),
                CheckOutDate = DateTime.Parse("2023-03-18"),
                BookingDate = DateTime.Parse("2023-03-05")
            }
        };
    }
}