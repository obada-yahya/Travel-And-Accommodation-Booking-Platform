using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Persistence.Repositories;

public class GuestRepository : IGuestRepository
{
    private readonly ApplicationDbContext _context;

    public GuestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Guest>> GetAllAsync()
    {
        try
        {
            return await _context
                .Guests
                .Include(guest => guest.Bookings)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception e)
        {
            return Enumerable.Empty<Guest>();
        }
    }

    public async Task<Guest?> GetByIdAsync(Guid guestId)
    {
        try
        {
            return await _context
                .Guests
                .SingleAsync(guest => guest.Id == guestId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }

    public async Task<Guest?> InsertAsync(Guest guest)
    {
        try
        {
            await _context.Guests.AddAsync(guest);
            await SaveChangesAsync();
            return guest;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task UpdateAsync(Guest guest)
    {
        _context.Guests.Update(guest);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid guestId)
    {
        var guestToRemove = new Guest { Id = guestId };
        _context.Guests.Remove(guestToRemove);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExistsAsync(Guid guestId)
    {
        return await _context
            .Payments
            .AnyAsync
            (guest => guest.Id.Equals(guestId));
    }
}