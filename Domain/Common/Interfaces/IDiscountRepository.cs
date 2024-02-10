using Domain.Common.Models;
using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IDiscountRepository
{
    public Task<PaginatedList<Discount>>
    GetAllByRoomTypeIdAsync(Guid roomTypeId, int pageNumber, int pageSize);
    public Task<Discount?> GetByIdAsync(Guid discountId);
    public Task<Discount?> InsertAsync(Discount discount);
    public Task DeleteAsync(Guid discountId);
    public Task SaveChangesAsync();
    public Task<bool> IsExistsAsync(Guid discountId);
}