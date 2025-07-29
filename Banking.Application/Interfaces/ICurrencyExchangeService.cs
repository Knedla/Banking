using Banking.Domain.ValueObjects;

namespace Banking.Application.Interfaces;

public interface ICurrencyExchangeService
{
    /// <summary>
    /// Converts the specified amount from one currency to another.
    /// </summary>
    Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency);

    Task<CurrencyAmount> ConvertAsync(decimal amount, ExchangeRate exchangeRate);

    Task<ExchangeRate> GetExchangeRateAsync(string fromCurrency, string toCurrency);
}
