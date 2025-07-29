namespace Banking.Application.Rules;

public class RuleDefinition
{
    public string RuleName { get; set; }
    public string EventType { get; set; }
    public string Condition { get; set; }
    public string Action { get; set; }
    public string Property { get; set; }
    public string Value { get; set; }
}
