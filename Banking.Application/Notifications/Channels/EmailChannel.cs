using Banking.Application.Notifications.Interfaces;

namespace Banking.Application.Notifications.Channels;

public class EmailChannel : INotificationChannel
{
    public string ChannelType => "Email";
    public Task SendAsync(string destination, string message)
    {
        // SMTP or EmailService.Send(...)
        return Task.CompletedTask;
    }
}
