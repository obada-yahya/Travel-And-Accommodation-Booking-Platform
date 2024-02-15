using Domain.Common.Models;
using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IRoomRepository
{
    public Task<PaginatedList<Room>> 
    GetAllAsync(string? searchQuery,
        int pageNumber,
        int pageSize);
    public Task<PaginatedList<Room>> 
    GetRoomsByHotelIdAsync(Guid hotelId,
        string? searchQuery,
        int pageNumber,
        int pageSize);
    public Task<bool> CheckRoomBelongsToHotelAsync(Guid hotelId,
        Guid roomId);
    public Task<Room?> GetByIdAsync(Guid roomId);
    public Task<Room?> InsertAsync(Room room);
    public Task UpdateAsync(Room room);
    public Task DeleteAsync(Guid roomId);
    public Task SaveChangesAsync();
    public Task<bool> IsExistsAsync(Guid roomId);
}