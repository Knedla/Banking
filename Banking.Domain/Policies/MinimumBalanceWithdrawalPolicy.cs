using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Plicies;
using Banking.Domain.Repositories;

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
            {
                return Task.FromResult(TransactionPolicyResult.Failure("Account not found."));
            }

            if (transaction.CalculatedCurrencyAmount.Amount > acount.Balances.First(s => s.CurrencyCode == transaction.CalculatedCurrencyAmount.CurrencyCode).AvailableBalance)
                return Task.FromResult(TransactionPolicyResult.Failure("Insufficient balance."));

            return Task.FromResult(TransactionPolicyResult.Success());
        }
    }
}
