using Banking.Application.Notifications.Rules;
using Banking.Domain.Entities;
using Banking.Domain.Events;

namespace Banking.Application.Interfaces;

public interface INotificationRuleProvider
{
    Task<IEnumerable<NotificationRule>> GetEffectiveRulesAsync(IDomainEvent domainEvent, IEnumerable<InvolvedPartyNotificationOverride> userOverrides);
}
