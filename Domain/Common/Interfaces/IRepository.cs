using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity
{
    public Task<IEnumerable<TEntity>> GetAllAsync();
    public Task<TEntity?> GetByIdAsync(Guid id);
    TEntity? InsertAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
}