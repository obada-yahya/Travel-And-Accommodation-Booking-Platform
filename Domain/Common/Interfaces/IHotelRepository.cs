using Domain.Common.Models;
using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IHotelRepository
{
    public Task<PaginatedList<Hotel>> 
    GetAllAsync(string? searchQuery, int pageNumber, int pageSize);
    public Task<Hotel?> GetByIdAsync(Guid hotelId);
    public Task<Hotel?> InsertAsync(Hotel hotel);
    public Task UpdateAsync(Hotel hotel);
    public Task DeleteAsync(Guid hotelId);
    public Task SaveChangesAsync();
    public Task<bool> IsExistsAsync(Guid hotelId);
}