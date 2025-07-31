using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Policies;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Policies;

public class InitializeTransactionAuthorizationPolicy : IInitializeTransactionAuthorizationPolicy
{
    private readonly IAccountRepository _accountRepository;

    public InitializeTransactionAuthorizationPolicy(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public Task<AuthorizationPolicyResult> EvaluateAsync(Guid involvedPartyId, Guid accountId, CancellationToken cancellationToken = default)
    {
        return (true)
            ? Task.FromResult(AuthorizationPolicyResult.Success())
            : Task.FromResult(AuthorizationPolicyResult.Failure($"You do not have initialization rights"));
    }
}
