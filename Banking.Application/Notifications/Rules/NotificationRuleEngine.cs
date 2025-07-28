using Banking.Application.Notifications.Interfaces;
using Banking.Domain.Events;

namespace Banking.Application.Notifications.Rules;

public class NotificationRuleEngine : INotificationRuleEngine
{
    private readonly List<NotificationRule> _rules;

    public NotificationRuleEngine(IOptions<List<NotificationRule>> options)
    {
        _rules = options.Value;
    }

    public async Task<List<NotificationRule>> EvaluateRulesAsync(IDomainEvent domainEvent)
    {
        var eventType = domainEvent.GetType().Name;
        var matchingRules = _rules
            .Where(r => r.EventType == eventType)
            .Where(r =>
            {
                if (string.IsNullOrWhiteSpace(r.Condition)) return true;
                return EvaluateCondition(r.Condition, domainEvent);
            }).ToList();

        return matchingRules;
    }

    private bool EvaluateCondition(string condition, IDomainEvent evt)
    {
        var context = new Dictionary<string, object>();
        foreach (var prop in evt.GetType().GetProperties())
        {
            context[prop.Name] = prop.GetValue(evt);
        }

        var expression = DynamicExpressionParser.ParseLambda(
            context.GetType(), typeof(bool), condition);

        return (bool)expression.Compile().DynamicInvoke(context);
    }
}
