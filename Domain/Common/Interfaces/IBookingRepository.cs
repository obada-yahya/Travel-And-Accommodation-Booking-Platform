using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IBookingRepository
{
    public Task<IReadOnlyList<Booking>>  GetAllAsync();
    public Task<Booking?> GetByIdAsync(Guid bookingId);
    public Task<Booking?> InsertAsync(Booking booking);
    public Task UpdateAsync(Booking booking);
    public Task DeleteAsync(Guid bookingId);
    public Task SaveChangesAsync();
    public Task<bool> CheckBookingExistenceForGuestAsync(Guid bookingId, string guestEmail);
    public Task<bool> IsExistsAsync(Guid bookingId);
}