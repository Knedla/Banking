using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Policies;
using Banking.Domain.Repositories;
using Banking.Infrastructure.Extensaions;

namespace Banking.Infrastructure.Policies;

public class MinimumBalanceWithdrawalPolicy : IWithdrawalPolicy
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyExchangeService _currencyExchangeService;

    public MinimumBalanceWithdrawalPolicy(IAccountRepository accountRepository, ICurrencyExchangeService currencyExchangeService)
    {
        _accountRepository = accountRepository;
        _currencyExchangeService = currencyExchangeService;
    }

    public async Task<TransactionPolicyResult> EvaluateAsync(Transaction transaction, Guid userId, CancellationToken cancellationToken = default)
    {
        var account = await transaction.FromTransactionAccountDetails.TryResolveAccount(_accountRepository);

        if (account == null)
            return TransactionPolicyResult.Failure("Account not found.");

        var primaryCurrencyCode = transaction.FromTransactionAccountDetails.Account.PrimaryCurrencyCode;
        var summary = transaction.SumByFromCurrencyCode();

        decimal availableBalance = 0;
        decimal requiredAmount = 0;
        foreach (var kv in summary)
        {
            var balance = account.Balances.FirstOrDefault(s => s.CurrencyCode == kv.Key);
            if (balance == null)
                return TransactionPolicyResult.Failure($"Balance {kv.Key} not found.");

            if (balance.CurrencyCode == primaryCurrencyCode)
            {
                availableBalance += balance.Balance;
                requiredAmount += balance.Balance;
                continue;
            }

            var exchangeRate = await _currencyExchangeService.GetExchangeRateAsync(kv.Key, primaryCurrencyCode);

            var convertedCurrencyAmount = await _currencyExchangeService.ConvertAsync(balance.Balance, exchangeRate);
            availableBalance += convertedCurrencyAmount.Amount;

            convertedCurrencyAmount = await _currencyExchangeService.ConvertAsync(kv.Value, exchangeRate);
            requiredAmount += convertedCurrencyAmount.Amount;
        }

        if (transaction.FromTransactionAccountDetails.Account.Overdraft != null)
        {
            if (transaction.FromTransactionAccountDetails.Account.Overdraft.CurrencyCode == primaryCurrencyCode)
                availableBalance += transaction.FromTransactionAccountDetails.Account.Overdraft.Limit;
            else
            {
                var amount = await _currencyExchangeService.ConvertAsync(
                    transaction.FromTransactionAccountDetails.Account.Overdraft.Limit, 
                    transaction.FromTransactionAccountDetails.Account.Overdraft.CurrencyCode,
                    primaryCurrencyCode);
                availableBalance += amount;
            }
        }

        return (requiredAmount > availableBalance) // error-prone due to multiple conversion, come up with a better solution or add some restrictions
            ? TransactionPolicyResult.Failure($"Insufficient balance {primaryCurrencyCode}: {requiredAmount}. ")
            : TransactionPolicyResult.Success();
    }
}
