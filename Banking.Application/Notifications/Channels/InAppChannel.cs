using Banking.Application.Notifications.Interfaces;

namespace Banking.Application.Notifications.Channels;

public class InAppChannel : INotificationChannel
{
    public string ChannelType => "InApp";
    public Task SendAsync(string destination, string message)
    {
        // 
        return Task.CompletedTask;
    }
}
