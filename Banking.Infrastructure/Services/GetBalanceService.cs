using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Services;

public class GetBalanceService : IGetBalanceService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthorizationPolicyService<IViewBalanceAuthorizationPolicy> _authorizationPolicyService;

    public GetBalanceService(
        IAccountRepository accountRepository, 
        IAuthorizationPolicyService<IViewBalanceAuthorizationPolicy> authorizationPolicyService)
    {
        _accountRepository = accountRepository;
        _authorizationPolicyService = authorizationPolicyService;
    }

    public async Task<AccountBalanceResponse> GetBalanceAsync(AccountBalanceRequest request)
    {
        if (request == null)
            throw new Exception($"Request is null.");

        var account = await _accountRepository.GetByIdAsync(request.AccountId);

        if (account == null)
            throw new Exception($"Cannot find account.");

        var authorizationPolicyResult = await _authorizationPolicyService.EvaluatePoliciesAsync(request.InvolvedPartyId, request.AccountId);
        if (authorizationPolicyResult.Any(s => !s.IsSuccess))
        {
            var response = new AccountBalanceResponse();
            foreach (var item in authorizationPolicyResult.Where(s => s!.IsSuccess))
                response.AddError(item.ErrorMessage);
            return response;
        }

        if (account.InvolvedPartyId != request.InvolvedPartyId)
        {
            var result = new AccountBalanceResponse();
            result.AddError("you do not have the right to see the account balance");
            return result;
        }

        return new AccountBalanceResponse
        {
            AccountId = account.Id,
            Balance = account.Balances.ToList()
        };
    }
}
