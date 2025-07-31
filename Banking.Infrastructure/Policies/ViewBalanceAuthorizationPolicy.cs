using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Policies;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Policies;

public class ViewBalanceAuthorizationPolicy : IViewBalanceAuthorizationPolicy
{
    private readonly IAccountRepository _accountRepository;

    public ViewBalanceAuthorizationPolicy(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public Task<AuthorizationPolicyResult> EvaluateAsync(Guid involvedPartyId, Guid accountId, CancellationToken cancellationToken = default)
    {
        // TODO:
        // add mandates to accounts
        // add rules for mandates
        // check if the user has mandate for the account
        // check if the user has the rule/right to view the balance

        return (true)
            ? Task.FromResult(AuthorizationPolicyResult.Success())
            : Task.FromResult(AuthorizationPolicyResult.Failure($"You do not have view balance rights"));
    }
}
