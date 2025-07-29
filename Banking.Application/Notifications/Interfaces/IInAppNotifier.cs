using Banking.Application.Notifications.Rules;

namespace Banking.Application.Notifications.Interfaces;

public interface IInAppNotifier
{
    Task NotifyAsync(Guid involvedPartyId, string message, NotificationPriority? priority);
}
