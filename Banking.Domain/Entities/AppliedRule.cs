namespace Banking.Domain.Entities;

public class AppliedRule // TODO: it is necessary to deepen the logic; currently the rules are only saved, nothing is done with them
{
    public string RuleName { get; set; }
    public string Condition { get; set; }
    public string Action { get; set; }
    public string Property { get; set; }
    public string Value { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}
