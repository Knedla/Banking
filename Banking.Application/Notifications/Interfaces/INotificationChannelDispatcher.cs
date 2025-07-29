using Banking.Application.Notifications.Rules;

namespace Banking.Application.Notifications.Interfaces;

public interface INotificationChannelDispatcher
{
    Task DispatchAsync(string channel, Guid userId, List<string> destination, string message, NotificationPriority? priority = null);
}
