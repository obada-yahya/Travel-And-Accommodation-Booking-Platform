using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface ICityRepository
{
    public Task<IEnumerable<City>> GetAllAsync();
    public Task<City?> GetByIdAsync(Guid cityId);
    Task<City?> InsertAsync(City city);
    Task UpdateAsync(City city);
    Task DeleteAsync(Guid cityId);
    Task SaveChangesAsync();
    
}