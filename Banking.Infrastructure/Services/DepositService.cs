using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;

namespace Banking.Infrastructure.Services;

public class DepositService : IDepositService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IInsertTransactionService<IDepositPolicy> _insertTransactionService;

    public DepositService(
        IAccountRepository accountRepository,
        IInsertTransactionService<IDepositPolicy> insertTransactionService)
    {
        _accountRepository = accountRepository;
        _insertTransactionService = insertTransactionService;
    }

    public async Task<DepositResponse> DepositAsync(DepositRequest request)
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

        if (!account.Balances.Any(s => s.CurrencyCode == request.CurrencyCode))
            throw new Exception($"Currency not {request.CurrencyCode} suported.");

        ExchangeRate exchangeRate = null;

        var fromCurrencyAmount = new CurrencyAmount() { Amount = request.Amount, CurrencyCode = request.CurrencyCode };
        var timestamp = DateTime.UtcNow;
        var transaction = new Transaction()
        {
            Id = Guid.NewGuid(), // TODO: implement IdGenereator
            TransactionInitializedById = request.TransactionInitializedById,
            // RelatedToTransactionId
            // ReversalTransactionId
            Timestamp = timestamp,
            Type = TransactionType.Deposit,
            Status = TransactionStatus.Pending,
            Channel = request.TransactionChannel,
            // Description
            ToTransactionAccountDetails = new TransactionAccountDetails()
            {
                AccountId = account.Id,
            },
            // CounterpartyAccountDetails

            FromCurrencyAmount = fromCurrencyAmount,
            // ExchangeRate
            CalculatedCurrencyAmount = fromCurrencyAmount,

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

        return await _insertTransactionService.AddAsync<DepositResponse>(transaction, request.UserId, CancellationToken.None);
    }
}
