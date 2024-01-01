using Domain.Entities;

namespace TAABP.Tests.InfrastructureTests.RepositoriesTests.TestData;

public class CityRepositoryTestData
{
    public static IEnumerable<object[]> CityRepositoryValidTestData
    {
        get
        {
            yield return new object[] { new City()
            {
                Id = Guid.NewGuid(),
                Name = "city1",
                CountryName = "ct1",
                CountryCode = "xyz",
                PostOffice = "PostOffice"
            }};
            yield return new object[] { new City()
            {
                Id = Guid.NewGuid(),
                Name = "city2",
                CountryName = "ct2",
                CountryCode = "xyz",
                PostOffice = "PostOffice"
            }};
            yield return new object[] { new City()
            {
                Id = Guid.NewGuid(),
                Name = "city3",
                CountryName = "ct3",
                CountryCode = "xyz",
                PostOffice = "PostOffice"
            }};
            yield return new object[] { new City()
            {
                Id = Guid.NewGuid(),
                Name = "city4",
                CountryName = "ct4",
                CountryCode = "xyz",
                PostOffice = "PostOffice"
            }};
            yield return new object[] { new City()
            {
                Id = Guid.NewGuid(),
                Name = "city5",
                CountryName = "ct5",
                CountryCode = "xyz",
                PostOffice = "PostOffice"
            }};
        }
    }
    
    public static IEnumerable<object[]> CitiesTestData()
    {
        yield return new object[]
        {
            new List<City>
            {
                new City { Id = Guid.NewGuid(), Name = "City1", CountryName = "ct1", CountryCode = "xyz", PostOffice = "PostOffice" },
                new City { Id = Guid.NewGuid(), Name = "City2", CountryName = "ct1", CountryCode = "xyz", PostOffice = "PostOffice" },
                new City { Id = Guid.NewGuid(), Name = "City3", CountryName = "ct1", CountryCode = "xyz", PostOffice = "PostOffice" },
                new City { Id = Guid.NewGuid(), Name = "City4", CountryName = "ct1", CountryCode = "xyz", PostOffice = "PostOffice" },
            }
        };
    }
}