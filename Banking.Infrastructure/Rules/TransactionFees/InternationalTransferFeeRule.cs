using Banking.Domain.Configuration;
using Banking.Domain.Entities;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Rules;

namespace Banking.Infrastructure.Rules.TransactionFees;

public class InternationalTransferFeeRule : ITransactionFeeRule
{
    private readonly TransactionFeeSettings _settings;

    public InternationalTransferFeeRule(TransactionFeeSettings settings)
    {
        _settings = settings;
    }

    public Task<bool> AppliesToAsync(Transaction transaction)
    {
        bool isInternational = !string.IsNullOrEmpty(transaction.RecipientDetails?.SwiftCode) || !string.IsNullOrEmpty(transaction.RecipientDetails?.Iban); // TODO: resolve isInternational
        return Task.FromResult(
            isInternational &&
            _settings.InternationalTransferFee.Where(s => s.CurrencyCode == transaction.CurrencyAmount.CurrencyCode).FirstOrDefault() != null); // refactor: double calculation: here and in GetFeeAsync
    }

    public async Task<Fee?> GetFeeAsync(Transaction transaction)
    {
        if (!await AppliesToAsync(transaction)) return null;

        var feeConfig = _settings.InternationalTransferFee.Where(s => s.CurrencyCode == transaction.CurrencyAmount.CurrencyCode).First();

        return new Fee
        {
            Code = "INT_TRF_FEE",
            Name = "International Transfer Fee",
            Amount = feeConfig.Amount,
            CurrencyCode = feeConfig.CurrencyCode,
            Type = feeConfig.Type,
            Trigger = FeeTrigger.OnExecution
        };
    }
}
