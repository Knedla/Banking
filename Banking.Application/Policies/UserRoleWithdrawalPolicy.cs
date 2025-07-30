using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Policies;

namespace Banking.Application.Policies;

public class UserRoleWithdrawalPolicy : IWithdrawalPolicy
{
    private readonly IUserContextService _userContext;

    public UserRoleWithdrawalPolicy(IUserContextService userContext)
    {
        _userContext = userContext;
    }

    public async Task<TransactionPolicyResult> EvaluateAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        var isAdmin = _userContext.IsInRole("Admin");
        return isAdmin
            ? TransactionPolicyResult.Success()
            : TransactionPolicyResult.Failure("Only Admins can withdraw large amounts.");
    }
}
