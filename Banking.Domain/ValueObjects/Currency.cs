using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.ValueObjects;

public class Currency
{
    [Required, StringLength(3)]
    public string Code { get; private set; }

    [Required, StringLength(3)]
    public string Symbol { get; private set; }

    [Required]
    public int DecimalPlaces { get; private set; }

    [Required, StringLength(50)]
    public string DisplayName { get; private set; }

    private static readonly Dictionary<string, (string Symbol, int Decimals, string Name)> SupportedCurrencies =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "EUR", ("€", 2, "Euro") },
            { "USD", ("$", 2, "US Dollar") },
            { "GBP", ("£", 2, "British Pound") },
            { "RSD", ("дин", 2, "Serbian Dinar") },
            { "CHF", ("Fr", 2, "Swiss Franc") }
        };

    public Currency(string code)
    {
        if (!SupportedCurrencies.TryGetValue(code.ToUpperInvariant(), out var currencyInfo))
            throw new ArgumentException($"Unsupported currency code: {code}");

        Code = code.ToUpperInvariant();
        Symbol = currencyInfo.Symbol;
        DecimalPlaces = currencyInfo.Decimals;
        DisplayName = currencyInfo.Name;
    }

    public override string ToString() => $"{Code} ({Symbol})";
    public static implicit operator string(Currency c) => c.Code;
    public static explicit operator Currency(string code) => new(code);
}
