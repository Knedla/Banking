using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Repositories;

public class TransferService : ITransferService
{
    private readonly IAccountRepository _accountRepository;

    public TransferService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<TransferResponse> TransferAsync(TransferRequest request)
    {
        var from = await _accountRepository.GetByIdAsync(request.FromAccountId);
        var to = await _accountRepository.GetByIdAsync(request.ToAccountId);

        from.Balance -= request.Amount;
        to.Balance += request.Amount;

        await _accountRepository.UpdateAsync(from);
        await _accountRepository.UpdateAsync(to);

        return new TransferResponse
        {
            FromAccountId = from.Id,
            ToAccountId = to.Id,
            FromNewBalance = from.Balance,
            ToNewBalance = to.Balance
        };
    }
}
