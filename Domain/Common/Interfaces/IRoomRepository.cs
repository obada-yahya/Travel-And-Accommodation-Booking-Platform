using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IRoomRepository
{
    public Task<IEnumerable<Room>> GetAllAsync();
    public Task<Room?> GetByIdAsync(Guid roomId);
    Task<Room?> InsertAsync(Room room);
    Task UpdateAsync(Room room);
    Task DeleteAsync(Guid roomId);
    Task SaveChangesAsync();
    Task<bool> IsExistsAsync(Guid roomId);
}