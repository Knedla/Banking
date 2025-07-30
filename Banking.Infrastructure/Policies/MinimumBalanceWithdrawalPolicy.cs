using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities.Accounts;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;
using Banking.Infrastructure.Extensaions;
using System.Text;

namespace Banking.Domain.Policies
{
    public class MinimumBalanceWithdrawalPolicy : IWithdrawalPolicy
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyExchangeService _currencyExchangeService;

        public MinimumBalanceWithdrawalPolicy(IAccountRepository accountRepository, ICurrencyExchangeService currencyExchangeService)
        {
            _accountRepository = accountRepository;
            _currencyExchangeService = currencyExchangeService;
        }

        public Task<TransactionPolicyResult> EvaluateAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
        {
            var account = _accountRepository.GetByIdAsync(transaction.AccountId).Result;

            if (account == null)
                return Task.FromResult(TransactionPolicyResult.Failure("Account not found."));

            var summary = transaction.SumByCurrencyCode();

            var stringBuilder = new StringBuilder();
            foreach (var kv in summary)
            {
                var balance = account.Balances.FirstOrDefault(s => s.CurrencyCode == kv.Key);
                if (balance == null)
                    return Task.FromResult(TransactionPolicyResult.Failure($"Balance {kv.Key} not found."));

                // OVERDRAFT AAAAAAAAAAAAAAAA !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                var availableBalance = 0; // await CalculateAvailableBalance(balance, account, account.Overdraft, null).Result;

                if (kv.Value > availableBalance)
                    stringBuilder.Append($"Insufficient balance {kv.Key}: {kv.Value}. ");
            }

            return stringBuilder.Length > 0 ?
                 Task.FromResult(TransactionPolicyResult.Failure(stringBuilder.ToString())) :
                 Task.FromResult(TransactionPolicyResult.Success());
        }

        async Task<decimal> CalculateAvailableBalance(AccountBalance accountBalance, Overdraft overdraft, ExchangeRate exchangeRate)
        {
            if (overdraft == null)
                return accountBalance.AvailableBalance;

            if (accountBalance.CurrencyCode == overdraft.CurrencyCode)
                return accountBalance.AvailableBalance + overdraft.Limit;

            if (exchangeRate == null ||
                exchangeRate.FromCurrency != overdraft.CurrencyCode ||
                exchangeRate.ToCurrency != accountBalance.CurrencyCode)
                throw new Exception("AccountBalanceExtension wrong exchange rate"); // error-prone, come up with a better solution or set restrictions so that the overdraft does not have to change currency

            var result = await _currencyExchangeService.ConvertAsync(overdraft.Limit, exchangeRate);
            return accountBalance.AvailableBalance + result.Amount;
        }
    }
}
