using Domain.Entities;
using FluentAssertions;
using Infrastructure.Common.Persistence;
using Infrastructure.Common.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using TAABP.Tests.InfrastructureTests.RepositoriesTests.TestData;

namespace TAABP.Tests.InfrastructureTests.RepositoriesTests;

public class HotelRepositoryTests
{
    private readonly ApplicationDbContext _context;

    public HotelRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new ApplicationDbContext(options.Options);
    }

    [Theory]
    [MemberData(nameof(HotelRepositoryTestData.HotelsTestData),
        MemberType = typeof(HotelRepositoryTestData))]
    [Trait("Category", "Hotel")]
    public async Task GetAllAsync_ShouldReturnAllHotels_WhenHotelsExist(List<Hotel> expectedHotels)
    {
        _context.Hotels.AddRange(expectedHotels);

        await _context.SaveChangesAsync();

        var sut = new HotelRepository(_context);

        var fetchedHotels = await sut.GetAllAsync();
        fetchedHotels.Should().HaveCount(expectedHotels.Count);
    }

    [Theory]
    [MemberData(nameof(HotelRepositoryTestData.HotelRepositoryValidTestData),
        MemberType = typeof(HotelRepositoryTestData))]
    [Trait("Category", "Hotel")]
    public async Task GetByIdAsync_ShouldReturnHotelWithMatchingId_WhenHotelWithMatchingIdExists(Hotel hotelToFind)
    {
        _context.Hotels.Add(hotelToFind);
        await _context.SaveChangesAsync();
        
        var sut = new HotelRepository(_context);

        var result = await sut.GetByIdAsync(hotelToFind.Id);
        
        result.Should().NotBeNull();
        result?.Id.Should().Be(hotelToFind.Id);
        result?.Rooms.Count.Should().Be(hotelToFind.Rooms.Count);
    }

    [Theory]
    [MemberData(nameof(HotelRepositoryTestData.HotelRepositoryValidTestData),
        MemberType = typeof(HotelRepositoryTestData))]
    [Trait("Category", "Hotel")]
    public async Task InsertAsync_ShouldInsertHotel_WhenHotelIsInserted(Hotel hotelToAdd)
    {
        var sut = new HotelRepository(_context);

        var result = await sut.InsertAsync(hotelToAdd);

        result.Should().NotBeNull();
        result?.Id.Should().NotBe(Guid.Empty);
        result?.Description.Should().Be(hotelToAdd.Description);
        
        var savedHotel = await _context
            .Hotels
            .FindAsync(result?.Id);
        
        savedHotel.Should().NotBeNull();
        savedHotel?.Description.Should().Be(hotelToAdd.Description);
    
    }

    [Theory]
    [MemberData(nameof(HotelRepositoryTestData.HotelRepositoryValidTestData),
        MemberType = typeof(HotelRepositoryTestData))]
    [Trait("Category", "Hotel")]
    public async Task UpdateAsync_ShouldUpdateHotel_WhenHotelIsUpdated(Hotel existingHotel)
    {
        await AddHotel(existingHotel);

        var updatedHotel = new Hotel
        {
            Id = existingHotel.Id, Name = "InteliSafe",
        };
        var sut = new HotelRepository(_context);

        await sut.UpdateAsync(updatedHotel);

        var result = await _context
            .Hotels
            .FindAsync(existingHotel.Id);
        
        result.Should().NotBeNull();
        result?.Name.Should().Be(updatedHotel.Name);
    }

    [Theory]
    [MemberData(nameof(HotelRepositoryTestData.HotelRepositoryValidTestData),
        MemberType = typeof(HotelRepositoryTestData))]
    [Trait("Category", "Hotel")]
    public async Task DeleteAsync_ShouldDeleteCity_WhenCityIsDeleted(Hotel hotelToDelete)
    {
        await AddHotel(hotelToDelete);
        var sut = new HotelRepository(_context);
        
        await sut.DeleteAsync(hotelToDelete.Id);
        
        var result = await _context
            .Guests
            .AsNoTracking()
            .SingleOrDefaultAsync
            (hotel => hotel.Id.Equals(hotelToDelete.Id));
        
        result.Should().BeNull();
    }

    private async Task AddHotel(Hotel hotel)
    {
        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();
        _context.Entry(hotel).State = EntityState.Detached;
    }
}