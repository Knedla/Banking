using Banking.Domain.Configuration;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Rules;
using Banking.Domain.ValueObjects;

namespace Banking.Infrastructure.Rules.TransactionFees;

public class WithdrawalFeeRule : ITransactionFeeRule
{
    private readonly TransactionFeeSettings _settings;

    public WithdrawalFeeRule(TransactionFeeSettings settings)
    {
        _settings = settings;
    }

    public Task<bool> AppliesToAsync(Transaction transaction)
    {
        string atmBankId = string.Empty; // TODO: resolve Atm Bank Id
        return Task.FromResult(
            transaction.Type == TransactionType.Withdrawal && 
            atmBankId != _settings.BankIdentifier &&
            _settings.NonSameBankWithdrawal.Where(s => s.CurrencyCode == transaction.FromCurrencyAmount.CurrencyCode).FirstOrDefault() != null); // refactor: double calculation: here and in GetFeeAsync
    }

    public async Task<Fee?> GetFeeAsync(Transaction transaction)
    {
        if (!await AppliesToAsync(transaction)) return null;

        var feeConfig = _settings.NonSameBankWithdrawal.Where(s => s.CurrencyCode == transaction.FromCurrencyAmount.CurrencyCode).First();

        var feeAmount = transaction.FromCurrencyAmount.Amount * feeConfig.Amount;

        return new Fee
        {
            Code = "EXT_ATM_WD_FEE",
            Name = "External ATM Withdrawal Fee",
            Amount = decimal.Round(feeAmount, 2),
            CurrencyCode = transaction.FromCurrencyAmount.CurrencyCode,
            AccountNumber = feeConfig.AccountNumber ?? _settings.DefaultAccountNumber,
            Type = FeeType.Percentage,
            Trigger = FeeTrigger.OnExecution
        };
    }
}
