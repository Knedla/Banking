using Banking.Domain.Configuration;
using Banking.Domain.Entities;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Rules;

namespace Banking.Infrastructure.Rules.TransactionFees;

public class ChannelFeeRule : ITransactionFeeRule
{
    private readonly TransactionFeeSettings _settings;

    public ChannelFeeRule(TransactionFeeSettings settings)
    {
        _settings = settings;
    }

    public Task<bool> AppliesToAsync(Transaction transaction)
    {
        return Task.FromResult(
            _settings.ChannelFees.ContainsKey(transaction.Channel.ToString()) && 
            _settings.ChannelFees[transaction.Channel.ToString()].Where(s => s.CurrencyCode == transaction.CurrencyAmount.CurrencyCode).FirstOrDefault() != null); // refactor: double calculation: here and in GetFeeAsync
    }

    public async Task<Fee?> GetFeeAsync(Transaction transaction)
    {
        if (!await AppliesToAsync(transaction)) return null;

        var channelFee = _settings.ChannelFees[transaction.Channel.ToString()].Where(s => s.CurrencyCode == transaction.CurrencyAmount.CurrencyCode).First();

        return new Fee
        {
            Code = $"CH_{transaction.Channel.ToString().ToUpper()}_FEE",
            Name = $"{transaction.Channel} Channel Fee",
            Amount = channelFee.Amount,
            CurrencyCode = channelFee.CurrencyCode,
            Type = channelFee.Type,
            Trigger = FeeTrigger.OnExecution
        };
    }
}
