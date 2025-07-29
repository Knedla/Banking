using Banking.Domain.Entities;

namespace Banking.Domain.Interfaces.Entities
{
    public interface IRuleEntity
    {
        ICollection<AppliedRule> AppliedRules { get; set; }
    }
}
