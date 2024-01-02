using Domain.Entities;

namespace TAABP.Tests.InfrastructureTests.RepositoriesTests.TestData;

public class HotelRepositoryTestData
{
     public static IEnumerable<object[]> HotelRepositoryValidTestData
    {
        get
        {
            yield return new object[] { new Hotel()
            {
                Id = Guid.Parse("d62a25b1-95c4-4c3a-8fc3-352b7f3991f5"),
                Name = "The Waldorf Astoria",
                PhoneNumber = "0592341234",
                Description = "n/a",
                StreetAddress = "n/a",
                Rooms = new List<Room>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        View = "You can see the ocean from the windows"
                    }
                }
            }};
        }
    }
    
    public static IEnumerable<object[]> HotelsTestData()
    {
        yield return new object[]
        {
            new List<Hotel>
            {
                new Hotel{ Id = Guid.NewGuid(), Name = "Xyz", Description = "n/a",PhoneNumber = "n/a",StreetAddress = "n/a"},
                new Hotel{ Id = Guid.NewGuid(), Name = "The Waldorf Astoria", Description = "n/a",PhoneNumber = "n/a",StreetAddress = "n/a"},
                new Hotel{ Id = Guid.NewGuid(), Name = "Raffles", Description = "n/a",PhoneNumber = "n/a",StreetAddress = "n/a"},
                new Hotel{ Id = Guid.NewGuid(), Name = "Belmond", Description = "n/a",PhoneNumber = "n/a",StreetAddress = "n/a"},
            }
        };
    }
}