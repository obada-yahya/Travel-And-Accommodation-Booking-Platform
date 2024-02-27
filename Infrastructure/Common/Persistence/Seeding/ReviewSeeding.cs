using Domain.Entities;

namespace Infrastructure.Common.Persistence.Seeding;

public class ReviewSeeding
{
    public static IEnumerable<Review> SeedData()
    {
        return new List<Review>
        {
            new()
            {
                Id = new Guid("63e4bb25-28b1-4fc4-9b93-9254d94dab23"),
                BookingId = new Guid("0bf4a177-98b8-4f67-8a56-95669c320890"),
                Comment = "Excellent service and comfortable stay!",
                ReviewDate = DateTime.Parse("2023-01-25"),
                Rating = 4.8f
            },
            new()
            {
                Id = new Guid("85a5a0b4-0e04-4c46-b7ac-6cf609e4f2aa"),
                BookingId = new Guid("efeb3d13-3dab-46c9-aa9a-9f22dd58e06e"),
                Comment = "Friendly staff and great location.",
                ReviewDate = DateTime.Parse("2023-02-10"),
                Rating = 4.5f
            },
            new()
            {
                Id = new Guid("192045db-c6db-49c9-aa6b-2e3d6c7f3b79"),
                BookingId = new Guid("7d3155a2-95f8-4d9b-bc24-662ae053f1c9"),
                Comment = "Clean rooms and beautiful views.",
                ReviewDate = DateTime.Parse("2023-03-18"),
                Rating = 4.2f
            }
        };
    }
}