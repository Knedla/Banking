namespace Banking.Domain.Configuration;

public class HighValueApprovalSettings
{
    public Dictionary<string, decimal> Thresholds { get; set; } = new();
}