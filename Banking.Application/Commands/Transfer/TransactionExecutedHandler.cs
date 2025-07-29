using Banking.Application.Events;
using Banking.Application.Notifications.Interfaces;
using Banking.Domain.Events;

namespace Banking.Application.Commands.Transfer;

public class TransactionExecutedHandler : IDomainEventHandler<TransactionExecutedEvent>
{
    private readonly INotificationEngine<TransactionExecutedEvent> _engine;

    public TransactionExecutedHandler(INotificationEngine<TransactionExecutedEvent> engine)
    {
        _engine = engine;
    }

    public Task HandleAsync(TransactionExecutedEvent domainEvent)
    {
        return _engine.HandleEventAsync(domainEvent);
    }
}
