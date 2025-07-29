namespace Banking.Application.Notifications.Interfaces;

public interface IEmailSender
{
    Task SendAsync(List<string> destination, string message);
}
