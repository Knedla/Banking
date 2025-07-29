using Banking.Application.Interfaces;
using Banking.Domain.Enumerations;

namespace Banking.Infrastructure.Services;

public class AccountNumberGenerator : IAccountNumberGenerator // TODO: implement real AccountNumberGenerator
{
    private static readonly Dictionary<AccountType, string> Prefixes = new()
    {
        { AccountType.Standard, "STA" },
        { AccountType.Checking, "CHK" },
        { AccountType.Savings,  "SAV" },
        { AccountType.Business, "BUS" },
        { AccountType.Loan,     "LON" },
        { AccountType.Credit,   "CRE" }
    };

    public Task<string> GenerateAsync(AccountType accountType, CancellationToken cancellationToken = default)
    {
        var prefix = Prefixes.TryGetValue(accountType, out var p) ? p : "GEN";
        var uniquePart = DateTime.UtcNow.ToString("yyMMddHHmmssfff"); // e.g. 250728214530123
        var random = new Random().Next(1000, 9999); // add entropy

        var accountNumber = $"{prefix}{uniquePart}{random}"; // e.g., CHK2507282145301235678
        return Task.FromResult(accountNumber);
    }
}
