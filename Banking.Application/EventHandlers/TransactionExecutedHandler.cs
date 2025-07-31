using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Application.Notifications.Interfaces;

namespace Banking.Application.EventHandlers;

public class TransactionExecutedHandler : IDomainEventHandler<TransactionExecutedEvent>
{
    private readonly INotificationEngine<TransactionExecutedEvent> _notificationEngine;

    public TransactionExecutedHandler(INotificationEngine<TransactionExecutedEvent> notificationEngine)
    {
        _notificationEngine = notificationEngine;
    }

    public Task HandleAsync(TransactionExecutedEvent domainEvent)
    {
        return _notificationEngine.HandleEventAsync(domainEvent);
    }
}
