using Domain.Entities;

namespace Infrastructure.Common.Persistence.Seeding;

public class GuestSeeding
{
    public static IEnumerable<Guest> SeedData()
    {
        return new List<Guest>
        {
            new Guest
            {
                Id = new Guid("c6c45f7c-2dfe-4c1e-9a9b-8b173c71b32c"),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890"
            },
            new Guest
            {
                Id = new Guid("aaf21a7d-8fc3-4c9f-8a8e-1eeec8dcd462"),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                PhoneNumber = "2012345678"
            },
            new Guest
            {
                Id = new Guid("f44c3eb4-2c8a-4a77-a31b-04c4619aa15a"),
                FirstName = "Hiroshi",
                LastName = "Tanaka",
                Email = "hiroshi.tanaka@example.co.jp",
                PhoneNumber = "312345678"
            }
        };
    }
}