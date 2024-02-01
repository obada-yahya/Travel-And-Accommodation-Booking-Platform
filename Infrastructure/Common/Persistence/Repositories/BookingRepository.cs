using Domain.Common.Interfaces;
using Domain.Common.Models;
using Domain.Entities;
using Domain.Exceptions;
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

    public async Task<PaginatedList<Booking>> GetAllByHotelIdAsync(
        Guid hotelId,
        int pageNumber,
        int pageSize)
    {
        try
        {
            var query = (from booking in _context.Bookings
                    join room in _context.Rooms on booking.RoomId equals room.Id
                    join roomType in _context.RoomTypes on room.RoomTypeId equals roomType.Id
                    join hotel in _context.Hotels on roomType.HotelId equals hotel.Id
                    where roomType.HotelId == hotelId
                    select booking)
                .AsQueryable()
                .AsNoTracking();

            var totalItemCount = await query.CountAsync();
            var pageData = new PageData(totalItemCount, pageSize, pageNumber);

            var result = query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToList();

            return new PaginatedList<Booking>(result, pageData);
        }
        catch (Exception)
        {
            return new PaginatedList<Booking>(new List<Booking>(), new PageData(0, 0, 0));
        }
    }

    private async Task<bool> CanBookRoom(Guid roomId, DateTime proposedCheckIn, DateTime proposedCheckOut)
    {
        var roomBookings = await _context
            .Bookings
            .Where(b => b.RoomId.Equals(roomId))
            .ToListAsync();

        return roomBookings.All(booking => 
            proposedCheckIn.Date > booking.CheckOutDate.Date || 
            proposedCheckOut.Date < booking.CheckInDate.Date);
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
        if (!await CanBookRoom(
                booking.RoomId,
                booking.CheckInDate,
                booking.CheckOutDate)) return null;
        
        await _context.Bookings.AddAsync(booking);
        await SaveChangesAsync();
        return booking;
    }

    public async Task UpdateAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid bookingId)
    {
        var bookingToRemove = await GetByIdAsync(bookingId);
        
        if (bookingToRemove!.CheckInDate.Date <= DateTime.Today.Date)
        {
            throw new BookingCheckInDatePassedException();
        }
        
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