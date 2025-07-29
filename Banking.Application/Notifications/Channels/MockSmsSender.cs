using Banking.Application.Notifications.Interfaces;

namespace Banking.Application.Notifications.Channels;

public class MockSmsSender : ISmsSender
{
    public Task SendAsync(List<string> destination, string message)
    {
        foreach (var item in destination)
        {
            // Use Twilio or similar
        }

        return Task.CompletedTask;
    }
}
