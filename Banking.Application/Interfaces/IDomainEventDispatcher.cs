using Banking.Domain.Interfaces;

namespace Banking.Application.Interfaces;

public interface IDomainEventDispatcher
{
    Task RaiseAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
}
