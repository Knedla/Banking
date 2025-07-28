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
        var account = await _accountRepository.GetByIdAsync(request.AccountId);
        return new AccountBalanceResponse
        {
            AccountId = account.Id,
            //Balance = account.Balance
        };
    }
}
