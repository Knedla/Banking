using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Repositories;

public class WithdrawService : IWithdrawService
{
    private readonly IAccountRepository _accountRepository;

    public WithdrawService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<WithdrawalResponse> WithdrawAsync(WithdrawalRequest request)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId);
        account.Balance -= request.Amount;

        await _accountRepository.UpdateAsync(account);

        return new WithdrawalResponse
        {
            AccountId = account.Id,
            NewBalance = account.Balance,
            ConfirmationNumber = Guid.NewGuid().ToString()
        };
    }
}
