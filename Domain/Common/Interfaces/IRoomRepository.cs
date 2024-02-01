using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IRoomRepository
{
    public Task<IReadOnlyList<Room>> GetAllAsync();
    public Task<Room?> GetByIdAsync(Guid roomId);
    public Task<Room?> InsertAsync(Room room);
    public Task UpdateAsync(Room room);
    public Task DeleteAsync(Guid roomId);
    public Task SaveChangesAsync();
    public Task<bool> IsExistsAsync(Guid roomId);
}