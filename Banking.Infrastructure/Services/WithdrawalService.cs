using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;

namespace Banking.Infrastructure.Services;

public class WithdrawalService : IWithdrawalService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionService _transactionService;

    public WithdrawalService(
        IAccountRepository accountRepository,
        ITransactionService transactionService)
    {
        _accountRepository = accountRepository;
        _transactionService = transactionService;
    }

    public async Task<WithdrawalResponse> WithdrawAsync(WithdrawalRequest request)
    {
        if (request == null)
            throw new Exception($"Request is null.");

        if (request.TransactionAccountDetails == null)
            throw new Exception($"TransactionAccountDetails is null.");

        var accountId = request.TransactionAccountDetails.AccountId.Value; // resolve id if needed

        if (accountId == Guid.Empty)
            throw new Exception($"AccountId is null.");

        var account = await _accountRepository.GetByIdAsync(accountId);

        if (account == null)
            throw new Exception($"Cannot find account.");

        if (!account.Balances.Any(s => s.CurrencyCode == request.CurrencyCode)) // it should never happen
            throw new Exception($"Currency not suported.");

        var currencyAmount = new CurrencyAmount() { Amount = request.Amount, CurrencyCode = request.CurrencyCode };
        var timestamp = DateTime.UtcNow;
        var transaction = new Transaction()
        {
            Id = Guid.NewGuid(), // TODO: implement IdGenereator
            TransactionInitializedById = request.TransactionInitializedById,
            // RelatedToTransactionId
            // ReversalTransactionId
            Timestamp = timestamp,
            Type = TransactionType.Withdrawal,
            Status = TransactionStatus.Pending,
            Channel = request.TransactionChannel,
            // Description
            FromTransactionAccountDetails = new TransactionAccountDetails()
            {
                AccountId = account.Id,
            },
            // CounterpartyAccountDetails

            FromCurrencyAmount = currencyAmount,
            // ExchangeRate
            CalculatedCurrencyAmount = currencyAmount,

            RequiresApproval = false,
            ApprovalStatus = ApprovalStatus.NotRequired,
            IsDeleted = false,
            CreatedAt = timestamp,
            CreatedByUserId = request.UserId,
            LastModifiedAt = timestamp,
            LastModifiedByUserId = request.UserId,

            // RelatedTransactions
            // ApprovalRequirements
            // Approvals
            // Batches
        };

        return await _transactionService.AddAsync<WithdrawalResponse>(transaction, request.UserId, CancellationToken.None);
    }
}
