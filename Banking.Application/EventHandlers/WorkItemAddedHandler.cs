using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Domain.Entities.WorkItems;

namespace Banking.Application.EventHandlers;

public abstract class WorkItemAddedHandler<T, U> : IDomainEventHandler<T> where T : WorkItemAddedEvent<U> where U : WorkItem // IDomainEventHandler<WorkItemAddedEvent<T>> where T : WorkItem
{
    private readonly IRuleProcessor _ruleProcessor;

    public WorkItemAddedHandler(IRuleProcessor ruleProcessor)
    {
        _ruleProcessor = ruleProcessor;
    }

    public async Task HandleAsync(T domainEvent)
    {
        await _ruleProcessor.ApplyRulesAsync(domainEvent.WorkItem);
    }
}
