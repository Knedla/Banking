using Banking.Domain.Entities.Parties;

namespace Banking.Application.Notifications.Routing;

public interface INotificationDestinationResolver
{
    Task<List<string>> ResolveAsync(string channelType, InvolvedParty involvedParty);
}
