using Banking.Domain.Entities.Transactions;
using Banking.Domain.Policies;

namespace Banking.Domain.Interfaces.Polices;

public interface IAuthorizationPolicy : IPolicy
{
    Task<AuthorizationPolicyResult> EvaluateAsync(Guid involvedPartyId, Guid accountId, CancellationToken cancellationToken = default);
}
