using Banking.Application.Notifications.Rules;
using Banking.Domain.Events;

namespace Banking.Application.Notifications.Interfaces;

public interface INotificationRuleEngine
{
    Task<List<NotificationRule>> EvaluateRulesAsync(IDomainEvent domainEvent);
}
