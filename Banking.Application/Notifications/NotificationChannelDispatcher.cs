using Banking.Application.Notifications.Interfaces;
using Banking.Application.Notifications.Rules;

namespace Banking.Application.Notifications;

public class NotificationChannelDispatcher : INotificationChannelDispatcher // TODO: refactor, poorly implemented
{
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;
    private readonly IInAppNotifier _inAppNotifier;

    public NotificationChannelDispatcher(
        IEmailSender emailSender,
        ISmsSender smsSender,
        IInAppNotifier inAppNotifier)
    {
        _emailSender = emailSender;
        _smsSender = smsSender;
        _inAppNotifier = inAppNotifier;
    }

    public async Task DispatchAsync(string channel, Guid userId, List<string> destination, string message, NotificationPriority? priority = null)
    {
        switch (channel.ToLowerInvariant())
        {
            case "email":
                await _emailSender.SendAsync(destination, message);
                break;
            case "sms":
                await _smsSender.SendAsync(destination, message);
                break;
            case "inapp":
                await _inAppNotifier.NotifyAsync(userId, message, priority);
                break;
            default:
                throw new NotSupportedException($"Notification channel '{channel}' is not supported.");
        }
    }
}
