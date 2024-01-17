using Domain.Common.Models;
using Domain.Entities;
namespace Domain.Common.Interfaces;

public interface ICityRepository
{
    public Task<PaginatedList<City>>
    GetAllAsync(bool includeHotels, int pageNumber, int pageSize);
    public Task<City?> GetByIdAsync(Guid cityId, bool includeHotels);
    public Task<City?> InsertAsync(City city);
    public Task UpdateAsync(City city);
    public Task DeleteAsync(Guid cityId);
    public Task SaveChangesAsync();
    public Task<bool> IsExistsAsync(Guid cityId);
}