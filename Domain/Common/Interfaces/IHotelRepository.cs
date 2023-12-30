using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IHotelRepository
{
    public Task<IEnumerable<Hotel>> GetAllAsync();
    public Task<Hotel?> GetByIdAsync(Guid hotelId);
    Task<Hotel?> InsertAsync(Hotel hotel);
    Task UpdateAsync(Hotel hotel);
    Task DeleteAsync(Guid hotelId);
    Task SaveChangesAsync();
    Task<bool> IsExistsAsync(Guid hotelId);
}