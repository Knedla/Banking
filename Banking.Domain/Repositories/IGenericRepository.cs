using Banking.Domain.Interfaces.Entities;

namespace Banking.Domain.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
