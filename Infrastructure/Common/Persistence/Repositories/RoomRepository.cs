using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Persistence.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDbContext _context;

    public RoomRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Room>> GetAllAsync()
    {
        try
        {
            return await _context
                .Rooms
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception e)
        {
            return Array.Empty<Room>();
        }
    }

    public async Task<Room?> GetByIdAsync(Guid roomId)
    {
        try
        {
            return await _context
                .Rooms
                .SingleAsync(room => room.Id.Equals(roomId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }

    public async Task<Room?> InsertAsync(Room room)
    {
        try
        {
            await _context.Rooms.AddAsync(room);
            await SaveChangesAsync();
            return room;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task UpdateAsync(Room room)
    {
        _context.Rooms.Update(room);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid roomId)
    {
        var roomToRemove = new Room { Id = roomId };
        _context.Rooms.Remove(roomToRemove);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExistsAsync(Guid roomId)
    {
        return await _context
            .Rooms
            .AnyAsync
            (room => room.Id.Equals(roomId));
    }
}