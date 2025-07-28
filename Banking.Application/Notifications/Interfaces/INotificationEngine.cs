using Banking.Domain.Events;

namespace Banking.Application.Notifications.Interfaces;

public interface INotificationEngine
{
    Task HandleEventAsync(IDomainEvent domainEvent);
}
