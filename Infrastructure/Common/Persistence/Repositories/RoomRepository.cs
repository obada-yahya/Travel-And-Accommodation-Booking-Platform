using Domain.Common.Interfaces;
using Domain.Common.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Common.Persistence.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RoomRepository> _logger;

    public RoomRepository(ApplicationDbContext context, ILogger<RoomRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    private async Task<PaginatedList<Room>> PaginateQueryAsync(IQueryable<Room> query, int pageNumber, int pageSize)
    {
        var totalItemCount = await query.CountAsync();
        var pageData = new PageData(totalItemCount, pageSize, pageNumber);

        var result = await query
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return new PaginatedList<Room>(result, pageData);
    }
    
    public async Task<PaginatedList<Room>> GetAllAsync(string? searchQuery, int pageNumber, int pageSize)
    {
        try
        {
            var query = _context.Rooms.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                query = query.Where(room => room.View.Contains(searchQuery));
            }
            return await PaginateQueryAsync(query, pageNumber, pageSize);
        }
        catch (Exception)
        {
            return new PaginatedList<Room>(new List<Room>(), new PageData(0, 0, 0));
        }
    }

    public async Task<PaginatedList<Room>> GetRoomsByHotelIdAsync(Guid hotelId, string? searchQuery, int pageNumber, int pageSize)
    {
        try
        {
            var query = (from roomType in _context.RoomTypes
                join room in _context.Rooms on roomType.Id equals room.RoomTypeId
                where roomType.HotelId == hotelId
                select room);
            
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                query = query.Where(room => room.View.Contains(searchQuery));
            }

            return await PaginateQueryAsync(query, pageNumber, pageSize);
        }
        catch (Exception)
        {
            return new PaginatedList<Room>(new List<Room>(), new PageData(0, 0, 0));
        }
    }

    public async Task<bool> CheckRoomBelongsToHotelAsync(Guid hotelId, Guid roomId)
    {
        return await (from roomType in _context.RoomTypes
            where roomType.HotelId.Equals(hotelId)
            join room in _context.Rooms on
            roomType.Id equals room.RoomTypeId 
            where room.Id.Equals(roomId) select room)
            .AnyAsync();
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
            _logger.LogError(e.Message);
        }
        return null;
    }

    public async Task<float> GetPriceForRoomWithDiscount(Guid roomId)
    {
        return await (from room in _context.Rooms
                where room.Id == roomId
                join roomType in _context.RoomTypes on room.RoomTypeId equals roomType.Id
                select roomType.PricePerNight * (1 - GetActiveDiscount(roomType.Discounts))
            ).SingleAsync();
    }

    private static float GetActiveDiscount(IEnumerable<Discount> roomType)
    {
        return roomType
            .FirstOrDefault(discount =>
                discount.FromDate.Date <= DateTime.Today.Date && 
                discount.ToDate.Date >= DateTime.Today.Date)
            ?.DiscountPercentage ?? 0.0f;
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
            _logger.LogError(e.Message);
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