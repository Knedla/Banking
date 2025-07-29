using Banking.Domain.Enumerations;

namespace Banking.Application.Interfaces;

public interface IAccountNumberGenerator
{
    Task<string> GenerateAsync(AccountType accountType, CancellationToken cancellationToken = default);
}
