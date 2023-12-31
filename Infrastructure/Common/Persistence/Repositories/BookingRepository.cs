using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Persistence.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Booking>> GetAllAsync()
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
        catch (Exception e)
        {
            return Enumerable.Empty<Booking>();
        }
    }

    public async Task<Booking?> GetByIdAsync(Guid bookingId)
    {
        try
        {
            return await _context
                .Bookings
                .SingleAsync(booking => booking.Id.Equals(bookingId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
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
            Console.WriteLine(e.Message);
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

    public async Task<bool> IsExistsAsync(Guid bookingId)
    {
        return await _context
            .Bookings
            .AnyAsync
            (booking => booking.Id.Equals(bookingId));
    }
}