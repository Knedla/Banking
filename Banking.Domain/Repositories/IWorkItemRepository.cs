using Banking.Domain.Entities.WorkItems;
using Banking.Domain.Enumerations;

namespace Banking.Domain.Repositories;

public interface IWorkItemRepository : IGenericRepository<WorkItem>
{
    Task UpdateStatusAsync(WorkItem entity, WorkItemStatus status);
}
