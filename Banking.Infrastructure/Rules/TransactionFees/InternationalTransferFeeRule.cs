﻿using Banking.Domain.Configuration;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Rules;
using Banking.Domain.ValueObjects;

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
        bool isInternational = false; // TODO: resolve isInternational
        return Task.FromResult(
            isInternational &&
            _settings.InternationalTransferFee.Where(s => s.CurrencyCode == transaction.FromCurrencyAmount.CurrencyCode).FirstOrDefault() != null); // refactor: double calculation: here and in GetFeeAsync
    }

    public async Task<Fee?> GetFeeAsync(Transaction transaction)
    {
        if (!await AppliesToAsync(transaction)) return null;

        var feeConfig = _settings.InternationalTransferFee.Where(s => s.CurrencyCode == transaction.FromCurrencyAmount.CurrencyCode).First();

        return new Fee
        {
            Code = "INT_TRF_FEE",
            Name = "International Transfer Fee",
            Amount = feeConfig.Amount,
            CurrencyCode = feeConfig.CurrencyCode,
            AccountId = feeConfig.AccountId ?? _settings.DefaultAccountId,
            Type = feeConfig.Type,
            Trigger = FeeTrigger.OnExecution
        };
    }
}
