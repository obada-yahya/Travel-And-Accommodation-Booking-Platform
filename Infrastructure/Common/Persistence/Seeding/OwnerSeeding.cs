using Domain.Entities;

namespace Infrastructure.Common.Persistence.Seeding;

public class OwnerSeeding
{
    public static List<Owner> SeedData()
    {
        return new List<Owner>
        {
            new Owner
            {
                Id = new Guid("a1d1aa11-12e7-4e0f-8425-67c1c1e62c2d"),
                FirstName = "Obada",
                LastName = "Yahya",
                Email = "obadayahya.an@gmail.com",
                PhoneNumber = "0598231234",
            },
            new Owner
            {
                Id = new Guid("77b2c30b-65d0-4ea7-8a5e-71e7c294f117"),
                FirstName = "Muathe",
                LastName = "Jamil",
                Email = "muathejamil@gmail.com",
                PhoneNumber = "0598242354",
            }
        };
    }
}