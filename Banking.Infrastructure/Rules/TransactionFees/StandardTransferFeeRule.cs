using Banking.Domain.Configuration;
using Banking.Domain.Entities;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Rules;

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
        bool isInternational = !string.IsNullOrEmpty(transaction.RecipientDetails?.SwiftCode) || !string.IsNullOrEmpty(transaction.RecipientDetails?.Iban); // TODO: resolve isInternational
        return Task.FromResult(
            transaction.Type == TransactionType.Transfer && 
            !isInternational &&
            _settings.StandardTransferFee.Where(s => s.CurrencyCode == transaction.CurrencyAmount.CurrencyCode).FirstOrDefault() != null); // refactor: double calculation: here and in GetFeeAsync
    }

    public async Task<Fee?> GetFeeAsync(Transaction transaction)
    {
        if (!await AppliesToAsync(transaction)) return null;
        
        var feeConfig = _settings.StandardTransferFee.Where(s => s.CurrencyCode == transaction.CurrencyAmount.CurrencyCode).First();

        return new Fee
        {
            Code = "STD_TRF_FEE",
            Name = "Standard Transfer Fee",
            Amount = feeConfig.Amount,
            CurrencyCode = feeConfig.CurrencyCode,
            Type = feeConfig.Type,
            Trigger = FeeTrigger.OnExecution
        };
    }
}
