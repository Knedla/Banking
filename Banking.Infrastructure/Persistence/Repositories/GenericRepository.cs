using Banking.Domain.Interfaces.Entities;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly IBankingDataStore _dataStore;
    protected readonly List<TEntity> _dbSet;

    public GenericRepository(IBankingDataStore dataStore)
    {
        _dataStore = dataStore;
        _dbSet = dataStore.Get<TEntity>();
    }

    public Task<TEntity?> GetByIdAsync(Guid id)
    {
        var entity = _dbSet.Find(s => s.Id == id);
        return Task.FromResult(entity);
    }

    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var list = _dbSet.ToList();
        return Task.FromResult<IEnumerable<TEntity>>(list);
    }

    public Task AddAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TEntity entity)
    {
        var oldEntity = _dbSet.Find(s => s.Id == entity.Id);
        if (oldEntity != null)
            _dbSet.Remove(oldEntity);

        _dbSet.Add(entity);
        return Task.CompletedTask;
    }

    public Task DeleteByIdAsync(Guid id)
    {
        var oldEntity = _dbSet.Find(s => s.Id == id);
        if (oldEntity != null)
            _dbSet.Remove(oldEntity);

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        var exists = _dbSet.Any(s => s.Id == id);
        return Task.FromResult(exists);
    }
}
