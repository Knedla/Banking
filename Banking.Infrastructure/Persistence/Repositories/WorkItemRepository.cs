using Banking.Domain.Entities.WorkItems;
using Banking.Domain.Enumerations;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Persistence.Repositories;

public class WorkItemRepository : GenericRepository<WorkItem>, IWorkItemRepository
{
    public WorkItemRepository(IBankingDataStore dataStore) : base(dataStore) { }

    public async Task UpdateStatusAsync(WorkItem entity, WorkItemStatus status)
    {
        var oldEntity = _dbSet.Find(s => s.Id == entity.Id);
        if (oldEntity != null)
            _dbSet.Remove(oldEntity);

        entity.Status = status;
        _dbSet.Add(entity);
    }
}
