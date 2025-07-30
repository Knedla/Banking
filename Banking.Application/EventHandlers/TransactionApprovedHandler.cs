using Banking.Application.Events;
using Banking.Application.Interfaces;

namespace Banking.Application.EventHandlers;

public class TransactionApprovedHandler : IDomainEventHandler<TransactionApprovedEvent> // not implemented
{
    public TransactionApprovedHandler() { }

    public Task HandleAsync(TransactionApprovedEvent domainEvent)
    {
        throw new NotImplementedException();
    }
}
