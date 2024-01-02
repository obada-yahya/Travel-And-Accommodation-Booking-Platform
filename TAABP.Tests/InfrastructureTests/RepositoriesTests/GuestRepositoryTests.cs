using Domain.Entities;
using Infrastructure.Common.Persistence;
using Infrastructure.Common.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using TAABP.Tests.InfrastructureTests.RepositoriesTests.TestData;
using FluentAssertions;

namespace TAABP.Tests.InfrastructureTests.RepositoriesTests;

public class GuestRepositoryTests
{
    private readonly ApplicationDbContext _context;

    public GuestRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new ApplicationDbContext(options.Options);
    }

    [Theory]
    [MemberData(nameof(GuestRepositoryTestData.GuestsTestData),
        MemberType = typeof(GuestRepositoryTestData))]
    [Trait("Category", "Guest")]
    public async Task GetAllAsync_ShouldReturnAllGuests_WhenGuestsExist(List<Guest> expectedGuests)
    {
        _context.Guests.AddRange(expectedGuests);

        await _context.SaveChangesAsync();

        var sut = new GuestRepository(_context);

        var fetchedGuests = await sut.GetAllAsync();
        fetchedGuests.Should().HaveCount(expectedGuests.Count);
    }

    [Theory]
    [MemberData(nameof(GuestRepositoryTestData.GuestRepositoryValidTestData),
        MemberType = typeof(GuestRepositoryTestData))]
    [Trait("Category", "Guest")]
    public async Task GetByIdAsync_ShouldReturnGuestWithMatchingId_WhenGuestWithMatchingIdExists(Guest guestToFind)
    {
        _context.Guests.Add(guestToFind);
        await _context.SaveChangesAsync();
        
        var sut = new GuestRepository(_context);

        var result = await sut.GetByIdAsync(guestToFind.Id);
        
        result.Should().NotBeNull();
        result?.Id.Should().Be(guestToFind.Id);
        result?.Bookings.Count.Should().Be(guestToFind.Bookings.Count);
    }

    [Theory]
    [MemberData(nameof(GuestRepositoryTestData.GuestRepositoryValidTestData),
        MemberType = typeof(GuestRepositoryTestData))]
    [Trait("Category", "Guest")]
    public async Task InsertAsync_ShouldInsertCity_WhenCityIsInserted(Guest guestToAdd)
    {
        var sut = new GuestRepository(_context);

        var result = await sut.InsertAsync(guestToAdd);

        result.Should().NotBeNull();
        result?.Id.Should().NotBe(Guid.Empty);
        result?.FirstName.Should().Be(guestToAdd.FirstName);
        
        var savedGuest = await _context
            .Guests
            .FindAsync(result?.Id);
        
        savedGuest.Should().NotBeNull();
        savedGuest?.FirstName.Should().Be(guestToAdd.FirstName);
    
    }

    [Theory]
    [MemberData(nameof(GuestRepositoryTestData.GuestRepositoryValidTestData),
        MemberType = typeof(GuestRepositoryTestData))]
    [Trait("Category", "Guest")]
    public async Task UpdateAsync_ShouldUpdateCity_WhenCityIsUpdated(Guest existingGuest)
    {
        await AddGuest(existingGuest);

        var updatedGuest = new Guest
        {
            Id = existingGuest.Id, FirstName = "UpdatedGuestName", LastName = "UpdatedGuestName",
        };
        var sut = new GuestRepository(_context);

        await sut.UpdateAsync(updatedGuest);

        var result = await _context
            .Guests
            .FindAsync(existingGuest.Id);
        
        result.Should().NotBeNull();
        result?.FirstName.Should().Be(updatedGuest.FirstName);
        result?.LastName.Should().Be(updatedGuest.LastName);
    }

    [Theory]
    [MemberData(nameof(GuestRepositoryTestData.GuestRepositoryValidTestData),
        MemberType = typeof(GuestRepositoryTestData))]
    [Trait("Category", "Guest")]
    public async Task DeleteAsync_ShouldDeleteCity_WhenCityIsDeleted(Guest guestToDelete)
    {
        await AddGuest(guestToDelete);
        var sut = new GuestRepository(_context);
        
        await sut.DeleteAsync(guestToDelete.Id);
        
        var result = await _context
            .Guests
            .AsNoTracking()
            .SingleOrDefaultAsync
            (guest => guest.Id.Equals(guestToDelete.Id));
        
        result.Should().BeNull();
    }

    private async Task AddGuest(Guest guest)
    {
        await _context.Guests.AddAsync(guest);
        await _context.SaveChangesAsync();
        _context.Entry(guest).State = EntityState.Detached;
    }
}