using Banking.Application.Interfaces;
using Banking.Application.Notifications.Rules;
using Banking.Domain.Entities;
using Banking.Domain.Events;

namespace Banking.Application.Notifications;

public class NotificationRuleProvider : INotificationRuleProvider
{
    private readonly IEnumerable<NotificationRule> _rules;

    public NotificationRuleProvider(IEnumerable<NotificationRule> rules)
    {
        _rules = rules;
    }

    public Task<IEnumerable<NotificationRule>> GetEffectiveRulesAsync(IDomainEvent domainEvent, IEnumerable<InvolvedPartyNotificationOverride> userOverrides)
    {
        string eventType = domainEvent.GetType().Name;

        var globalRules = _rules.Where(r => r.EventType == eventType).ToList();

        var effectiveRules = new List<NotificationRule>();

        foreach (var globalRule in globalRules)
        {
            var userOverride = userOverrides.FirstOrDefault(u => u.EventType == globalRule.EventType);

            if (userOverride is { IsEnabled: false })
                continue; // Skip disabled

            var modifiedRule = new NotificationRule
            {
                EventType = globalRule.EventType,
                Channels = userOverride?.Channels ?? globalRule.Channels,
                Condition = userOverride?.Condition ?? globalRule.Condition,
                MessageTemplate = userOverride?.MessageTemplate ?? globalRule.MessageTemplate,
                Priority = userOverride?.Priority ?? globalRule.Priority
            };

            effectiveRules.Add(modifiedRule);
        }

        return Task.FromResult<IEnumerable<NotificationRule>>(effectiveRules);
    }
}
