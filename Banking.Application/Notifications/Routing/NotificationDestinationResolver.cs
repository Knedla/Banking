using Banking.Domain.Entities.Parties;
using Banking.Domain.Enumerations;

namespace Banking.Application.Notifications.Routing;

public class NotificationDestinationResolver : INotificationDestinationResolver
{
    public Task<List<string>> ResolveAsync(string channelType, InvolvedParty involvedParty)
    {
        if (involvedParty == null)
            return null;

        List<string> result = null;
        if (channelType == "Email")
            result = GetEmails(involvedParty);
        else if (channelType == "SMS")
            result = GetSMS(involvedParty);
        else if (channelType == "InApp")
            result = new List<string>() { involvedParty.Id.ToString() };

        return Task.FromResult(result);
    }

    List<string> GetEmails(InvolvedParty involvedParty)
    {
        var emails = involvedParty.Emails?.Where(s => s.Purposes.HasFlag(ContactPurpose.Billing)).ToList();
        if (emails == null)
            return null;

        return emails.Select(s => s.Email).ToList();
    }

    List<string> GetSMS(InvolvedParty involvedParty)
    {
        var phoneNumbers = involvedParty.PhoneNumbers?.Where(s => s.Purposes.HasFlag(ContactPurpose.Billing)).ToList();
        if (phoneNumbers == null)
            return null;

        return phoneNumbers.Select(s => s.Number).ToList(); // TODO: get real number
    }
}
