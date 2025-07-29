using Banking.Domain.Events;

namespace Banking.Application.Notifications.Interfaces;

public interface INotificationEngine<TEvent> where TEvent : IDomainEvent
{
    Task HandleEventAsync(TEvent domainEvent);
}
