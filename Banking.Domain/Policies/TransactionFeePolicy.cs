using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Interfaces.Rules;
using Banking.Domain.ValueObjects;

namespace Banking.Domain.Policies;

public class TransactionFeePolicy : ITransactionFeePolicy
{
    private readonly IEnumerable<ITransactionFeeRule> _feeRules;

    public TransactionFeePolicy(IEnumerable<ITransactionFeeRule> feeRules)
    {
        _feeRules = feeRules;
    }

    public async Task<List<Fee>> EvaluateAsync(Transaction transaction, CancellationToken cancellationToken = default)
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
