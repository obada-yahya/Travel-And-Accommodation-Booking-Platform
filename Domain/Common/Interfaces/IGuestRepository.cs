using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IGuestRepository
{
    public Task<IEnumerable<Guest>> GetAllAsync();
    public Task<Guest?> GetByIdAsync(Guid guestId);
    Task<Guest?> InsertAsync(Guest guest);
    Task UpdateAsync(Guest guest);
    Task DeleteAsync(Guid guestId);
    Task SaveChangesAsync();
    Task<bool> IsExistsAsync(Guid guestId);
}