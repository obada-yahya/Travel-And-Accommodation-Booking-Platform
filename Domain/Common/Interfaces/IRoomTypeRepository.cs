using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IRoomTypeRepository
{
    public Task<IReadOnlyList<RoomType>> GetAllAsync();
    public Task<RoomType?> GetByIdAsync(Guid roomTypeId);
    Task<RoomType?> InsertAsync(RoomType roomType);
    Task UpdateAsync(RoomType roomType);
    Task DeleteAsync(Guid roomTypeId);
    Task SaveChangesAsync();
    Task<bool> IsExistsAsync(Guid roomTypeId);
}