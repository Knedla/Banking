using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Services;

public class GetBalanceService : IGetBalanceService
{
    private readonly IAccountRepository _accountRepository;

    public GetBalanceService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountBalanceResponse> GetBalanceAsync(AccountBalanceRequest request)
    {
        if (request == null)
            throw new Exception($"Request is null.");

        var account = await _accountRepository.GetByIdAsync(request.AccountId);

        if (account == null)
            throw new Exception($"Cannot find account.");

        // TODO:
        // add mandates to accounts
        // add rules for mandates
        // check if the user has mandate for the account
        // check if the user has the rule/right to view the balance

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
