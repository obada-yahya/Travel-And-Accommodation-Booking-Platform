namespace Domain.Common.Interfaces;

public interface IRepository<T> where T : class
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T?> GetByIdAsync(int id);
    T? InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}