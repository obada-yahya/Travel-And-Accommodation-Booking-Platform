using Domain.Common.Models;
using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IRoomAmenityRepository
{
    public Task<PaginatedList<RoomAmenity>>
    GetAllAsync(string? searchQuery, int pageNumber, int pageSize);
    public Task<RoomAmenity?> GetByIdAsync(Guid amenityId);
    public Task<RoomAmenity?> InsertAsync(RoomAmenity roomAmenity);
    public Task UpdateAsync(RoomAmenity roomAmenity);
    public Task DeleteAsync(Guid amenityId);
    public Task SaveChangesAsync();
    public Task<bool> IsExistsAsync(Guid amenityId);
}