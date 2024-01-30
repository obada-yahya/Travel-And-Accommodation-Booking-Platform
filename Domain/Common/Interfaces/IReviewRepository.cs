using Domain.Common.Models;
using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IReviewRepository
{
    public Task<PaginatedList<Review>> GetAllByHotelIdAsync(Guid hotelId, string? searchQuery, int pageNumber, int pageSize);
    public Task<Review?> GetByIdAsync(Guid reviewId);
    public Task<Review?> InsertAsync(Review review);
    public Task UpdateAsync(Review review);
    public Task DeleteAsync(Guid reviewId);
    public Task<bool> DoesBookingHaveReviewAsync(Guid bookingId);
    public Task SaveChangesAsync();
    public Task<bool> IsExistsAsync(Guid reviewId);
}