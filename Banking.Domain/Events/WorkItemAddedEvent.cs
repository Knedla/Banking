using Banking.Domain.Entities.WorkItems;
using Banking.Domain.Interfaces;

namespace Banking.Application.Events;

public abstract class WorkItemAddedEvent<T> : IDomainEvent where T : WorkItem
{
    public T WorkItem { get; }

    public Guid InvolvedPartyId { get; set; }

    public WorkItemAddedEvent(T workItem)
    {
        WorkItem = workItem;
    }
}
