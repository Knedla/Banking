using Banking.Domain.Configuration;
using Banking.Domain.Entities;
using Banking.Domain.Entities.Accounts;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Rules;

namespace Banking.Infrastructure.Rules.TransactionFees;

public class MinimumBalanceFeeRule : ITransactionFeeRule
{
    private readonly TransactionFeeSettings _settings;

    public MinimumBalanceFeeRule(TransactionFeeSettings settings)
    {
        _settings = settings;
    }

    public Task<bool> AppliesToAsync(Transaction transaction)
    {
        var minimumBalanceThreshold = _settings.MinimumBalanceThreshold.Where(s => s.CurrencyCode == transaction.InitCurrencyAmount.CurrencyCode).FirstOrDefault();

        if (minimumBalanceThreshold == null)
            return Task.FromResult(false);

        AccountBalance accountBalance = null; // TODO: get accountBalance
        if (accountBalance == null)
            throw new ArgumentNullException(nameof(MinimumBalanceFeeRule));

        return Task.FromResult(
            accountBalance.AvailableBalance - transaction.CalculatedCurrencyAmount.Amount < minimumBalanceThreshold.Amount &&
            _settings.MinimumBalanceFee.Where(s => s.CurrencyCode == transaction.InitCurrencyAmount.CurrencyCode).FirstOrDefault() != null); // refactor: double calculation: here and in GetFeeAsync
    }

    public async Task<Fee?> GetFeeAsync(Transaction transaction)
    {
        if (!await AppliesToAsync(transaction)) return null;

        var feeConfig = _settings.MinimumBalanceFee.Where(s => s.CurrencyCode == transaction.InitCurrencyAmount.CurrencyCode).First();

        return new Fee
        {
            Code = "MIN_BAL_FEE",
            Name = "Minimum Balance Fee",
            Amount = feeConfig.Amount,
            CurrencyCode = feeConfig.CurrencyCode,
            Type = feeConfig.Type,
            Trigger = FeeTrigger.OnExecution
        };
    }
}
