using Banking.Application.Events;
using Banking.Application.Interfaces;

namespace Banking.Application.EventHandlers;

public class TransactionApprovedHandler : IDomainEventHandler<TransactionApprovedEvent> // mock - like a generic TransactionApprovedHandler
{
    public TransactionApprovedHandler() { }

    public Task HandleAsync(TransactionApprovedEvent domainEvent)
    {
        return Task.CompletedTask;
    }
}
