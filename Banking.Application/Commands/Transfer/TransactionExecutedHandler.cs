using Banking.Application.Events;
using Banking.Application.Notifications.Interfaces;
using Banking.Domain.Events;

namespace Banking.Application.Commands.Transfer;

public class TransactionExecutedHandler : IDomainEventHandler<TransactionExecutedEvent>
{
    private readonly INotificationEngine _notificationEngine;

    public TransactionExecutedHandler(INotificationEngine notificationEngine)
    {
        _notificationEngine = notificationEngine;
    }

    public async Task HandleAsync(TransactionExecutedEvent @event)
    {
        await _notificationEngine.HandleEventAsync(@event);
    }
}
