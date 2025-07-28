using Banking.Domain.Events;

namespace Banking.Application.Common;

public interface IDomainEventDispatcher
{
    Task RaiseAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
}
