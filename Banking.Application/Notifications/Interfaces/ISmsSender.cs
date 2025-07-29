namespace Banking.Application.Notifications.Interfaces;

public interface ISmsSender
{
    Task SendAsync(List<string> destination, string message);
}