using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Common.Persistence.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BookingRepository> _logger;

    public BookingRepository(ApplicationDbContext context, ILogger<BookingRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<IReadOnlyList<Booking>> GetAllAsync()
    {
        try
        {
            return await _context
                .Bookings
                .Include(booking => booking.Payment)
                .Include(booking => booking.Review)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception)
        {
            return Array.Empty<Booking>();
        }
    }

    public async Task<Booking?> GetByIdAsync(Guid bookingId)
    {
        try
        {
            var bookings = _context.Bookings.ToList();
            foreach (var booking in bookings)
            {
                _logger.LogInformation($"Look at : {booking.Id}");
            }
            return await _context
                .Bookings
                .SingleAsync(booking => booking.Id.Equals(bookingId));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
        return null;
    }

    public async Task<Booking?> InsertAsync(Booking booking)
    {
        try
        {
            await _context.Bookings.AddAsync(booking);
            await SaveChangesAsync();
            return booking;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task UpdateAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid bookingId)
    {
        var bookingToRemove = new Booking { Id = bookingId };
        _context.Bookings.Remove(bookingToRemove);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckBookingExistenceForGuestAsync(Guid bookingId, string guestEmail)
    {
        var booking = await GetByIdAsync(bookingId);
        
        var guest = await _context
                        .Users
                        .SingleAsync
                        (guest =>
                        guest.Email
                        .Equals(guestEmail));
        
        return booking!.UserId.Equals(guest.Id);
    }

    public async Task<bool> IsExistsAsync(Guid bookingId)
    {
        return await _context
            .Bookings
            .AnyAsync
            (booking => booking.Id.Equals(bookingId));
    }
}