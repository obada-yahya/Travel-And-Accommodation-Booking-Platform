using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IBookingRepository
{
    public Task<IEnumerable<Booking>> GetAllAsync();
    public Task<Booking?> GetByIdAsync(Guid bookingId);
    Task<Booking?> InsertAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task DeleteAsync(Guid bookingId);
    Task SaveChangesAsync();
    Task<bool> IsExistsAsync(Guid bookingId);
}