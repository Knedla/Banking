using Banking.Application.Interfaces.Services;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Policies;

namespace Banking.Application.Policies;

public class AuthorizationPolicyService<TPolicy> : IAuthorizationPolicyService<TPolicy> where TPolicy : IAuthorizationPolicy
{
    private readonly IEnumerable<TPolicy> _policies;

    public AuthorizationPolicyService(IEnumerable<TPolicy> policies)
    {
        _policies = policies;
    }

    public async Task<List<AuthorizationPolicyResult>> EvaluatePoliciesAsync(
        Guid involvedPartyId, 
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var results = new List<AuthorizationPolicyResult>();

        foreach (var policy in _policies)
        {
            var result = await policy.EvaluateAsync(involvedPartyId, accountId, cancellationToken);
            results.Add(result);
        }

        return results;
    }
}