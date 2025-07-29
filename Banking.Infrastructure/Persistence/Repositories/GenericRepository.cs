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

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return _dbSet.Find(s => s.Id == id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return _dbSet.ToList();
    }

    public async Task AddAsync(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        var oldEntity = _dbSet.Find(s => s.Id == entity.Id);
        if (oldEntity != null)
            _dbSet.Remove(oldEntity);
        _dbSet.Add(entity);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var oldEntity = _dbSet.Find(s => s.Id == id);
        if (oldEntity != null)
            _dbSet.Remove(oldEntity);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return _dbSet.Any(s => s.Id == id);
    }
}
