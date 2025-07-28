using Banking.Application.Notifications.Interfaces;

namespace Banking.Application.Notifications.Channels;

public class SmsChannel : INotificationChannel
{
    public string ChannelType => "SMS";
    public Task SendAsync(string destination, string message)
    {
        // Use Twilio or similar
        return Task.CompletedTask;
    }
}
