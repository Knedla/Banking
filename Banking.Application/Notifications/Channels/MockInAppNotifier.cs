using Banking.Application.Notifications.Interfaces;
using Banking.Application.Notifications.Rules;

namespace Banking.Application.Notifications.Channels;

public class MockInAppNotifier : IInAppNotifier
{
    public Task NotifyAsync(Guid involvedPartyId, string message, NotificationPriority? priority)
    {
        return Task.CompletedTask;
    }
}
