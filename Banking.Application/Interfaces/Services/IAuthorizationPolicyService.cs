using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Policies;

namespace Banking.Application.Interfaces.Services;

// not happiest solution organized as this
// IViewBalanceAuthorizationPolicy
// IInitializeTransactionAuthorizationPolicy
public interface IAuthorizationPolicyService<TPolicy> where TPolicy : IAuthorizationPolicy
{
    Task<List<AuthorizationPolicyResult>> EvaluatePoliciesAsync(Guid involvedPartyId, Guid accountId, CancellationToken cancellationToken = default);
}