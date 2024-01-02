using Domain.Entities;
using FluentAssertions;
using Infrastructure.Common.Persistence;
using Infrastructure.Common.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using TAABP.Tests.InfrastructureTests.RepositoriesTests.TestData;

namespace TAABP.Tests.InfrastructureTests.RepositoriesTests;

public class CityRepositoryTests
{
    private readonly ApplicationDbContext _context;

    public CityRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new ApplicationDbContext(options.Options);
    }
    
    [Theory]
    [MemberData(nameof(CityRepositoryTestData.CitiesTestData),
        MemberType = typeof(CityRepositoryTestData))]
    [Trait("Category", "City")]
    public async Task GetAllAsync_ShouldReturnAllCities_WhenCitiesExist(List<City> expectedCities)
    {
        _context.Cities.AddRange(expectedCities);

        await _context.SaveChangesAsync();

        var sut = new CityRepository(_context);

        var fetchedCities = await sut.GetAllAsync();
        
        fetchedCities.Should().HaveCount(expectedCities.Count);
    }

    [Theory]
    [MemberData(nameof(CityRepositoryTestData.CityRepositoryValidTestData),
        MemberType = typeof(CityRepositoryTestData))]
    [Trait("Category", "City")]
    public async Task GetByIdAsync_ShouldReturnCityWithMatchingId_WhenCityWithMatchingIdExists(City cityToFind)
    {
        await AddCity(cityToFind);

        var sut = new CityRepository(_context);

        var result = await sut.GetByIdAsync(cityToFind.Id);
        
        result.Should().NotBeNull();
        result?.Id.Should().Be(cityToFind.Id);
        result?.Name.Should().Be(cityToFind.Name);
    }

    [Theory]
    [MemberData(nameof(CityRepositoryTestData.CityRepositoryValidTestData),
        MemberType = typeof(CityRepositoryTestData))]
    [Trait("Category", "City")]
    public async Task InsertAsync_ShouldInsertCity_WhenCityIsInserted(City cityToAdd)
    {
        var sut = new CityRepository(_context);

        var result = await sut.InsertAsync(cityToAdd);

        result.Should().NotBeNull();
        result?.Id.Should().NotBe(Guid.Empty);
        result?.Name.Should().Be(cityToAdd.Name);

        var savedCity = await _context
            .Cities
            .FindAsync(result?.Id);
        
        savedCity.Should().NotBeNull();
        savedCity?.Name.Should().Be(cityToAdd.Name);
    }

    [Theory]
    [MemberData(nameof(CityRepositoryTestData.CityRepositoryValidTestData),
        MemberType = typeof(CityRepositoryTestData))]
    [Trait("Category", "City")]
    public async Task UpdateAsync_ShouldUpdateCity_WhenCityIsUpdated(City existingCity)
    {
        await AddCity(existingCity);

        var updatedCity = new City
        {
            Id = existingCity.Id, Name = "UpdatedCity", CountryName = "ct1", CountryCode = "xyz",
            PostOffice = "PostOffice"
        };
        var sut = new CityRepository(_context);
        
        await sut.UpdateAsync(updatedCity);

        var result = await _context
            .Cities
            .FindAsync(existingCity.Id);
        
        result.Should().NotBeNull();
        result?.Name.Should().Be(updatedCity.Name);
    }

    [Theory]
    [MemberData(nameof(CityRepositoryTestData.CityRepositoryValidTestData),
        MemberType = typeof(CityRepositoryTestData))]
    [Trait("Category", "City")]
    public async Task DeleteAsync_ShouldDeleteCity_WhenCityIsDeleted(City cityToDelete)
    {
        await AddCity(cityToDelete);
        var sut = new CityRepository(_context);
        
        await sut.DeleteAsync(cityToDelete.Id);
        
        var result = await _context
            .Cities
            .AsNoTracking()
            .SingleOrDefaultAsync
            (city => cityToDelete.Id.Equals(city.Id));

        result.Should().BeNull();
    }

    private async Task AddCity(City city)
    {
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();
        _context.Entry(city).State = EntityState.Detached;
    }
}