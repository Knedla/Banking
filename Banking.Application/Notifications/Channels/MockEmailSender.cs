using Banking.Application.Notifications.Interfaces;

namespace Banking.Application.Notifications.Channels;

public class MockEmailSender : IEmailSender
{
    public Task SendAsync(List<string> destination, string message)
    {
        foreach (var recipient in destination)
        {
            // SMTP or EmailService.Send(...)
        }

        return Task.CompletedTask;
    }
}
