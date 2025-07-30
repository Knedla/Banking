using Banking.Domain.Configuration;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Rules;
using Banking.Domain.ValueObjects;

namespace Banking.Infrastructure.Rules.TransactionFees;

public class StandardTransferFeeRule : ITransactionFeeRule
{
    private readonly TransactionFeeSettings _settings;

    public StandardTransferFeeRule(TransactionFeeSettings settings)
    {
        _settings = settings;
    }

    public Task<bool> AppliesToAsync(Transaction transaction)
    {
        bool isInternational = !string.IsNullOrEmpty(transaction.CounterpartyAccountDetails?.SwiftCode) || !string.IsNullOrEmpty(transaction.CounterpartyAccountDetails?.Iban); // TODO: resolve isInternational
        return Task.FromResult(
            transaction.Type == TransactionType.Transfer && 
            !isInternational &&
            _settings.StandardTransferFee.Where(s => s.CurrencyCode == transaction.InitCurrencyAmount.CurrencyCode).FirstOrDefault() != null); // refactor: double calculation: here and in GetFeeAsync
    }

    public async Task<Fee?> GetFeeAsync(Transaction transaction)
    {
        if (!await AppliesToAsync(transaction)) return null;
        
        var feeConfig = _settings.StandardTransferFee.Where(s => s.CurrencyCode == transaction.InitCurrencyAmount.CurrencyCode).First();

        return new Fee
        {
            Code = "STD_TRF_FEE",
            Name = "Standard Transfer Fee",
            Amount = feeConfig.Amount,
            CurrencyCode = feeConfig.CurrencyCode,
            AccountNumber = feeConfig.AccountNumber ?? _settings.DefaultAccountNumber,
            Type = feeConfig.Type,
            Trigger = FeeTrigger.OnExecution
        };
    }
}
