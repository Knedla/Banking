using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;
using System.Text;

namespace Banking.Domain.Policies
{
    public class MinimumBalanceWithdrawalPolicy : IWithdrawalPolicy
    {
        private readonly IAccountRepository _accountRepository;

        public MinimumBalanceWithdrawalPolicy(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<TransactionPolicyResult> EvaluateAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
        {
            var acount = _accountRepository.GetByIdAsync(transaction.AccountId).Result;

            if (acount == null)
                return Task.FromResult(TransactionPolicyResult.Failure("Account not found."));

            var summary = SumByCurrencyCode(transaction);

            var stringBuilder = new StringBuilder();
            foreach (var kv in summary)
            {
                if (kv.Value > acount.Balances.First(s => s.CurrencyCode == kv.Key).AvailableBalance)
                    stringBuilder.Append($"Insufficient balance {kv.Key}: {kv.Value}. ");
            }

            return stringBuilder.Length > 0 ?
                 Task.FromResult(TransactionPolicyResult.Failure(stringBuilder.ToString())) :
                 Task.FromResult(TransactionPolicyResult.Success());
        }

        private Dictionary<string, decimal> SumByCurrencyCode(Transaction transaction)
        {
            var allAmounts = new List<CurrencyAmount>
            {
                transaction.CalculatedCurrencyAmount
            };

            // Add related transactions' CurrencyAmounts
            if (transaction.RelatedTransactions != null)
                foreach (var related in transaction.RelatedTransactions)
                    allAmounts.Add(related.CalculatedCurrencyAmount);

            // Group by CurrencyCode and sum the Amounts
            return allAmounts
                .GroupBy(x => x.CurrencyCode)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => x.Amount)
                );
        }
    }
}
