using Domain.Entities;
namespace Domain.Common.Interfaces;

public interface ICityRepository
{
    public Task<(IReadOnlyList<City>, PaginationMetaData)>
    GetAllAsync(bool includeHotels, int pageNumber, int pageSize);
    public Task<City?> GetByIdAsync(Guid cityId, bool includeHotels);
    Task<City?> InsertAsync(City city);
    Task UpdateAsync(City city);
    Task DeleteAsync(Guid cityId);
    Task SaveChangesAsync();
    Task<bool> IsExistsAsync(Guid cityId);
}