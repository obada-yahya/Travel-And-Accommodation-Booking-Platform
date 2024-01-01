using Domain.Entities;
using Infrastructure.Common.Persistence;
using Infrastructure.Common.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using TAABP.Tests.InfrastructureTests.RepositoriesTests.TestData;
using Xunit.Abstractions;

namespace TAABP.Tests.InfrastructureTests.RepositoriesTests;

public class CityRepositoryTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ApplicationDbContext _context;

    public CityRepositoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        // Should Comment OnConfiguring Method to work
        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new ApplicationDbContext(options.Options);
    }

    [Theory]
    [MemberData(nameof(CityRepositoryTestData.CitiesTestData),
        MemberType = typeof(CityRepositoryTestData))]
    [Trait("Category", "Queries")]
    public async Task GetAllAsync_ShouldReturnAllCities(List<City> expectedCities)
    {
        // Arrange

        // Act
        _context.Cities.AddRange(expectedCities);

        await _context.SaveChangesAsync();

        var sut = new CityRepository(_context);

        var fetchedCities = await sut.GetAllAsync();

        // Assert 
        Assert.Equal(expectedCities.Count, fetchedCities.Count());
    }

    [Theory]
    [MemberData(nameof(CityRepositoryTestData.CityRepositoryValidTestData),
        MemberType = typeof(CityRepositoryTestData))]
    [Trait("Category", "Queries")]
    public async Task GetByIdAsync_ShouldReturnCityWithMatchingId(City cityToFind)
    {
        // Arrange
        _context.Cities.Add(cityToFind);
        await _context.SaveChangesAsync();

        var sut = new CityRepository(_context);

        // Act
        var result = await sut.GetByIdAsync(cityToFind.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cityToFind.Id, result?.Id);
        Assert.Equal(cityToFind.Name, result?.Name);
    }

    [Theory]
    [MemberData(nameof(CityRepositoryTestData.CityRepositoryValidTestData),
        MemberType = typeof(CityRepositoryTestData))]
    [Trait("Category", "Commands")]
    public async Task InsertAsync_ShouldInsertCity(City cityToAdd)
    {
        // Arrange
        var sut = new CityRepository(_context);

        // Act
        var result = await sut.InsertAsync(cityToAdd);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result?.Id);
        Assert.Equal(cityToAdd.Name, result?.Name);

        // Check if the city is actually in the database
        var savedCity = await _context.Cities.FindAsync(result?.Id);
        Assert.NotNull(savedCity);
        Assert.Equal(cityToAdd.Name, savedCity?.Name);
    }

    [Theory]
    [MemberData(nameof(CityRepositoryTestData.CityRepositoryValidTestData),
        MemberType = typeof(CityRepositoryTestData))]
    [Trait("Category", "Commands")]
    public async Task UpdateAsync_ShouldUpdateCity(City existingCity)
    {
        // Arrange
        _context.Cities.Add(existingCity);
        await _context.SaveChangesAsync();
        _context.Entry(existingCity).State = EntityState.Detached;

        var updatedCity = new City
        {
            Id = existingCity.Id, Name = "UpdatedCity", CountryName = "ct1", CountryCode = "xyz",
            PostOffice = "PostOffice"
        };
        var sut = new CityRepository(_context);

        // Act
        await sut.UpdateAsync(updatedCity);

        // Assert
        var result = await _context.Cities.FindAsync(existingCity.Id);
        Assert.NotNull(result);
        Assert.Equal(updatedCity.Name, result?.Name);
    }

    [Theory]
    [MemberData(nameof(CityRepositoryTestData.CityRepositoryValidTestData),
        MemberType = typeof(CityRepositoryTestData))]
    [Trait("Category", "Commands")]
    public async Task DeleteAsync_ShouldDeleteCity(City cityToDelete)
    {
        // Arrange
        await _context.Cities.AddAsync(cityToDelete);
        await _context.SaveChangesAsync();
        _context.Entry(cityToDelete).State = EntityState.Detached;

        var sut = new CityRepository(_context);

        // Act
        var cityExistsBeforeDeletion = await _context.Cities.AnyAsync(city => city.Id == cityToDelete.Id);

        if (cityExistsBeforeDeletion)
        {
            await sut.DeleteAsync(cityToDelete.Id);
        }
        else
        {
            _testOutputHelper.WriteLine($"City with ID {cityToDelete.Id} does not exist. Skipping deletion.");
        }

        // Assert
        _testOutputHelper.WriteLine(cityToDelete.Id.ToString());
        var result = await _context.Cities.AsNoTracking().SingleOrDefaultAsync(city => cityToDelete.Id.Equals(city.Id));
        Assert.Null(result);
    }
}