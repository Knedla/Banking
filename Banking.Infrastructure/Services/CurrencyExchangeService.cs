using Banking.Application.Interfaces;

namespace Banking.Infrastructure.Services;

public class CurrencyExchangeService : ICurrencyExchangeService
{
    private readonly IDictionary<(string from, string to), decimal> _exchangeRates;

    public CurrencyExchangeService()
    {
        // In real usage, load from DB or external API
        _exchangeRates = new Dictionary<(string, string), decimal>
        {
            { ("EUR", "RSD"), 117.5m },
            { ("USD", "RSD"), 107.0m },
            { ("RSD", "EUR"), 1 / 117.5m },
            { ("RSD", "USD"), 1 / 107.0m },
            { ("EUR", "USD"), 1.1m },
            { ("USD", "EUR"), 0.91m }
        };
    }

    public Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency)
    {
        if (fromCurrency == toCurrency)
            return Task.FromResult(amount);

        if (_exchangeRates.TryGetValue((fromCurrency.ToUpper(), toCurrency.ToUpper()), out var rate))
            return Task.FromResult(amount * rate);

        throw new InvalidOperationException($"Exchange rate not found for {fromCurrency} to {toCurrency}.");
    }
}
