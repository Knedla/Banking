using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Services;

public class WithdrawalService : IWithdrawalService
{
    private readonly IAccountRepository _accountRepository;

    public WithdrawalService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<WithdrawalResponse> WithdrawAsync(WithdrawalRequest request)
    {
        if (request == null)
            throw new Exception($"Request is null.");

        var account = await _accountRepository.GetByIdAsync(request.AccountId);
        //account.Balance -= request.Amount;

        await _accountRepository.UpdateAsync(account);

        return new WithdrawalResponse
        {
            AccountId = account.Id,
            //NewBalance = account.Balance,
            ConfirmationNumber = Guid.NewGuid().ToString()
        };
    }
}
