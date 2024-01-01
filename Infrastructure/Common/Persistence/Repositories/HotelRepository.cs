using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Persistence.Repositories;

public class HotelRepository: IHotelRepository
{
    private readonly ApplicationDbContext _context;

    public HotelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Hotel>> GetAllAsync()
    {
        try
        {
            return await _context
                .Hotels
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception)
        {
            return Array.Empty<Hotel>();
        }
    }

    public async Task<Hotel?> GetByIdAsync(Guid hotelId)
    {
        try
        {
            return await _context
                .Hotels
                .SingleAsync(hotel => hotel.Id.Equals(hotelId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }

    public async Task<Hotel?> InsertAsync(Hotel hotel)
    {
        try
        {
            await _context.Hotels.AddAsync(hotel);
            await SaveChangesAsync();
            return hotel;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task UpdateAsync(Hotel hotel)
    {
        _context.Hotels.Update(hotel);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var hotelToRemove = new Hotel { Id = id };
        _context.Hotels.Remove(hotelToRemove);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExistsAsync(Guid hotelId)
    {
        return await _context
            .Hotels
            .AnyAsync
            (hotel => hotel.Id.Equals(hotelId));
    }
}