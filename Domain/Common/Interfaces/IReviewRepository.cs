using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IReviewRepository
{
    public Task<IEnumerable<Review>> GetAllAsync();
    public Task<Review?> GetByIdAsync(Guid reviewId);
    Task<Review?> InsertAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Guid reviewId);
    Task SaveChangesAsync();
    Task<bool> IsExistsAsync(Guid reviewId);
}