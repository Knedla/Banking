using Banking.Domain.Entities;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Rules;

namespace Banking.Domain.Policies;

public class TransactionFeePolicy
{
    private readonly IEnumerable<ITransactionFeeRule> _feeRules;

    public TransactionFeePolicy(IEnumerable<ITransactionFeeRule> feeRules)
    {
        _feeRules = feeRules;
    }

    public async Task<List<Fee>> EvaluateAsync(Transaction transaction)
    {
        var fees = new List<Fee>();

        foreach (var rule in _feeRules)
        {
            if (await rule.AppliesToAsync(transaction))
            {
                var fee = await rule.GetFeeAsync(transaction);
                if (fee != null)
                {
                    fees.Add(fee);
                }
            }
        }

        return fees;
    }
}
