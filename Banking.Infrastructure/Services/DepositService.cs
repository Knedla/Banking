using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Repositories;

public class DepositService : IDepositService
{
    private readonly IAccountRepository _accountRepository;

    public DepositService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<DepositResponse> DepositAsync(DepositRequest request)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId);
        account.Balance += request.Amount;

        await _accountRepository.UpdateAsync(account);

        return new DepositResponse
        {
            AccountId = account.Id,
            NewBalance = account.Balance
        };
    }
}
