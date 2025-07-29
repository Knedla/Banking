using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Banking.Domain.Interfaces.Entities;

namespace Banking.Application.Rules;

public class RuleProcessor : IRuleProcessor
{
    private readonly RuleDefinitions _settings;
    private readonly IExpressionEvaluator _evaluator;

    public RuleProcessor(RuleDefinitions settings, IExpressionEvaluator evaluator)
    {
        _settings = settings;
        _evaluator = evaluator;
    }

    public async Task ApplyRulesAsync<T>(T context) where T : class
    {
        var eventType = typeof(T).Name;

        foreach (var rule in _settings.Rules.Where(r => r.EventType == eventType))
        {
            if (_evaluator.Evaluate(rule.Condition, context))
            {
                var prop = context.GetType().GetProperty(rule.Property);
                if (prop != null && prop.CanWrite)
                {
                    object value = Convert.ChangeType(rule.Value, prop.PropertyType);
                    prop.SetValue(context, value);
                }

                if (context is IRuleEntity ruleEntity)
                {
                    if (ruleEntity.AppliedRules == null)
                        ruleEntity.AppliedRules = new List<AppliedRule>();

                    ruleEntity.AppliedRules.Add(new AppliedRule
                    {
                        RuleName = rule.RuleName,
                        Condition = rule.Condition,
                        Action = rule.Action,
                        Property = rule.Property,
                        Value = rule.Value
                    });
                }
            }
        }
    }
}