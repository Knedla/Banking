using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;

namespace Banking.Infrastructure.Services;

public class DepositService : IDepositService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyExchangeService _currencyExchangeService;
    private readonly ITransactionService _transactionService;

    public DepositService(
        IAccountRepository accountRepository,
        ICurrencyExchangeService currencyExchangeService,
        ITransactionService transactionService)
    {
        _accountRepository = accountRepository;
        _currencyExchangeService = currencyExchangeService;
        _transactionService = transactionService;
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

        ExchangeRate exchangeRate = null;

        if (string.IsNullOrEmpty(request.ToCurrencyCode))
            if (!account.Balances.Any(s => s.CurrencyCode == request.FromCurrencyCode))
                // maybe throw an exception instead of falling back on PrimaryCurrencyCode
                // -> throw new Exception($"Currency not suported."); 
                // maybe load decision from config or ask customers what they want
                request.ToCurrencyCode = account.PrimaryCurrencyCode;

        if (!string.IsNullOrEmpty(request.ToCurrencyCode))
        {
            if (!account.Balances.Any(s => s.CurrencyCode == request.ToCurrencyCode))
                throw new Exception($"Currency not suported.");

            exchangeRate = await _currencyExchangeService.GetExchangeRateAsync(request.FromCurrencyCode, request.ToCurrencyCode);

            if (exchangeRate == null)
                throw new Exception($"ExchangeRate not found.");
        }

        var fromCurrencyAmount = new CurrencyAmount() { Amount = request.Amount, CurrencyCode = request.FromCurrencyCode };
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
            ExchangeRate = exchangeRate,
            CalculatedCurrencyAmount = (exchangeRate == null) ? fromCurrencyAmount : await _currencyExchangeService.ConvertAsync(fromCurrencyAmount.Amount, exchangeRate),

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

        return await _transactionService.AddAsync<DepositResponse>(transaction, request.UserId, CancellationToken.None);
    }
}
