using Banking.Application.Events;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Notifications.Interfaces;
using Banking.Domain.Enumerations;
using Banking.Domain.Events;
using System.Reflection;

namespace Banking.Application.Notifications;

public class NotificationEngine : INotificationEngine
{
    private readonly INotificationRuleEngine _ruleEngine;
    private readonly IEnumerable<INotificationChannel> _channels;
    private readonly IInvolvedPartyService _involvedPartyService;

    public NotificationEngine(
        INotificationRuleEngine ruleEngine,
        IEnumerable<INotificationChannel> channels,
        IInvolvedPartyService involvedPartyService)
    {
        _ruleEngine = ruleEngine;
        _channels = channels;
        _involvedPartyService = involvedPartyService;
    }

    public async Task HandleEventAsync(IDomainEvent domainEvent)
    {
        var rules = await _ruleEngine.EvaluateRulesAsync(domainEvent);

        if (!rules.Any()) return;

        var transactionExecutedEvent = domainEvent as TransactionExecutedEvent;
        if (transactionExecutedEvent == null) return;

        var involvedPartyResponse = await _involvedPartyService.GetInvolvedPartyAsync(new InvolvedPartyRequest() { InvolvedPartyId = transactionExecutedEvent.InvolvedPartyId });

        if (involvedPartyResponse.InvolvedParty == null) return; // throw ex

        foreach (var rule in rules)
        {
            var message = ApplyTemplate(rule.MessageTemplate, domainEvent);

            foreach (var channelType in rule.Channels)
            {
                var channel = _channels.FirstOrDefault(c => c.ChannelType == channelType);

                if (channel == null) continue;

                List<string> destinations = null;

                if (channelType == "Email")
                {
                    var emails = involvedPartyResponse.InvolvedParty.Emails.Where(s => s.Purposes.Any(q => q == ContactPurpose.Billing)).ToList();
                    destinations = emails.Select(s => s.Email).ToList();
                }
                else if (channelType == "SMS")
                {
                    var phoneNumbers = involvedPartyResponse.InvolvedParty.PhoneNumbers.Where(s => s.Purposes.Any(q => q == ContactPurpose.Billing)).ToList();
                    destinations = phoneNumbers.Select(s => s.Number).ToList(); // TODO: get real number
                }

                if (destinations == null || destinations.Count == 0) continue;

                foreach (var destination in destinations)
                {
                    await channel.SendAsync(destination, message);
                }
            }
        }
    }

    private string ApplyTemplate(string template, IDomainEvent domainEvent)
    {
        var result = template;
        foreach (var prop in domainEvent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            result = result.Replace("{" + prop.Name + "}", prop.GetValue(domainEvent)?.ToString());
        }
        return result;
    }
}
