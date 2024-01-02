using Domain.Entities;
using FluentAssertions;
using Infrastructure.Common.Persistence;
using Infrastructure.Common.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using TAABP.Tests.InfrastructureTests.RepositoriesTests.TestData;

namespace TAABP.Tests.InfrastructureTests.RepositoriesTests;

public class BookingRepositoryTests
{
    private readonly ApplicationDbContext _context;

    public BookingRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new ApplicationDbContext(options.Options);
    }

    [Theory]
    [MemberData(nameof(BookingRepositoryTestData.BookingsTestData),
        MemberType = typeof(BookingRepositoryTestData))]
    [Trait("Category", "Booking")]
    public async Task GetAllAsync_ShouldReturnAllBookings_WhenBookingsExist(List<Booking> expectedBookings)
    {
        _context.Bookings.AddRange(expectedBookings);
    
        await _context.SaveChangesAsync();
    
        var sut = new BookingRepository(_context);
    
        var fetchedBookings = await sut.GetAllAsync();
        fetchedBookings.Should().HaveCount(expectedBookings.Count);
    }

    [Theory]
    [MemberData(nameof(BookingRepositoryTestData.BookingRepositoryValidTestData),
        MemberType = typeof(BookingRepositoryTestData))]
    [Trait("Category", "Booking")]
    public async Task GetByIdAsync_ShouldReturnBookingWithMatchingId_WhenBookingWithMatchingIdExists(Booking bookingToFind)
    {
        _context.Bookings.Add(bookingToFind);
        await _context.SaveChangesAsync();

        var sut = new BookingRepository(_context);

        var result = await sut.GetByIdAsync(bookingToFind.Id);

        result.Should().NotBeNull();
        result?.Id.Should().Be(bookingToFind.Id);
    }

    [Theory]
    [MemberData(nameof(BookingRepositoryTestData.BookingRepositoryValidTestData),
        MemberType = typeof(BookingRepositoryTestData))]
    [Trait("Category", "Booking")]
    public async Task InsertAsync_ShouldInsertBooking_WhenBookingIsInserted(Booking bookingToAdd)
    {
        var sut = new BookingRepository(_context);

        var result = await sut.InsertAsync(bookingToAdd);

        result.Should().NotBeNull();
        result?.Id.Should().NotBe(Guid.Empty);
        
        var savedBooking = await _context
            .Bookings
            .FindAsync(result?.Id);
        
        savedBooking.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(BookingRepositoryTestData.BookingRepositoryValidTestData),
        MemberType = typeof(BookingRepositoryTestData))]
    [Trait("Category", "Booking")]
    public async Task UpdateAsync_ShouldUpdateBooking_WhenBookingIsUpdated(Booking existingBooking)
    {
        await AddBooking(existingBooking);

        var updatedBooking = new Booking
        {
            Id = existingBooking.Id, BookingDate = DateTime.Now.AddDays(11)
        };
        var sut = new BookingRepository(_context);

        await sut.UpdateAsync(updatedBooking);

        var result = await _context
            .Bookings
            .FindAsync(existingBooking.Id);
        
        result.Should().NotBeNull();
        result?.BookingDate.Should().Be(updatedBooking.BookingDate);
    }

    [Theory]
    [MemberData(nameof(BookingRepositoryTestData.BookingRepositoryValidTestData),
        MemberType = typeof(BookingRepositoryTestData))]
    [Trait("Category", "Booking")]
    public async Task DeleteAsync_ShouldDeleteBooking_WhenBookingIsDeleted(Booking bookingToDelete)
    {
        await AddBooking(bookingToDelete);
        var sut = new BookingRepository(_context);
        
        await sut.DeleteAsync(bookingToDelete.Id);
        
        var result = await _context
            .Cities
            .AsNoTracking()
            .SingleOrDefaultAsync
            (booking => bookingToDelete.Id.Equals(booking.Id));

        result.Should().BeNull();
    }

    private async Task AddBooking(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
        _context.Entry(booking).State = EntityState.Detached;
    }
}