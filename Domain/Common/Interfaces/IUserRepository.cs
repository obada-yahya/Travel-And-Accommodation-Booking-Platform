using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IUserRepository
{
    public Task InsertAsync(User user);
    public Task<IReadOnlyList<User>> GetAllAsync();
    public Task<User?> GetByIdAsync(Guid userId);
    public Task UpdateAsync(User user);
    public Task DeleteAsync(Guid userId);
    public Task<bool> IsExistsAsync(Guid userId);
    public Task<Guid> GetGuestIdByEmailAsync(string email);
    public Task<List<Hotel>> GetRecentlyVisitedHotelsForGuestAsync(Guid guestId, int count);
    public Task<List<Hotel>> GetRecentlyVisitedHotelsForAuthenticatedGuestAsync(string email, int count);
    public Task<List<Booking>> GetBookingsForAuthenticatedGuestAsync(string email, int count);
    public Task SaveChangesAsync();
}