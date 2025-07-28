namespace Banking.Application.Notifications.Interfaces;

public interface INotificationChannel
{
    Task SendAsync(string destination, string message);
    string ChannelType { get; }
}
