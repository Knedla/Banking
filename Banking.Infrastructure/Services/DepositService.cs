using Banking.Application.Interfaces;
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

    public DepositService(
        IAccountRepository accountRepository,
        ICurrencyExchangeService currencyExchangeService)
    {
        _accountRepository = accountRepository;
        _currencyExchangeService = currencyExchangeService;
    }

    public async Task<DepositResponse> DepositAsync(DepositRequest request)
    {
        if (request == null)
            throw new Exception($"Request is null.");

        if (request.AccountId == Guid.Empty) // TODO: or any alternative key
            throw new Exception($"AccountId is null.");

        var account = await _accountRepository.GetByIdAsync(request.AccountId);

        if (account == null)
            throw new Exception($"Cannot find account.");

        ExchangeRate exchangeRate = null;

        if (!string.IsNullOrEmpty(request.ToCurrencyCode))
        {
            if (!account.Balances.Any(s => s.CurrencyCode == request.ToCurrencyCode))
                throw new Exception($"Currency not suported.");

            exchangeRate = await _currencyExchangeService.GetExchangeRateAsync(request.FromCurrencyCode, request.ToCurrencyCode);
        }
        else
        {
            if (!account.Balances.Any(s => s.CurrencyCode == request.FromCurrencyCode))
                throw new Exception($"Currency not suported.");
        }

        var initCurrencyAmount = new CurrencyAmount() { Amount = request.Amount, CurrencyCode = request.FromCurrencyCode };
        var timestamp = DateTime.UtcNow;
        var transactionId = Guid.NewGuid(); // TODO: implement IdGenereator
        var transaction = new Transaction()
        {
            Id = transactionId,
            // RelatedToTransactionId
            // ReversalTransactionId
            Timestamp = timestamp,
            Type = TransactionType.Deposit,
            Status = TransactionStatus.Pending,
            Channel = request.TransactionChannel,
            AccountId = account.Id,
            // Description
            // RecipientDetails

            InitCurrencyAmount = initCurrencyAmount,
            ExchangeRate = exchangeRate,
            CalculatedCurrencyAmount = (exchangeRate == null) ? initCurrencyAmount : await _currencyExchangeService.ConvertAsync(initCurrencyAmount.Amount, exchangeRate),

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

        // trigger transaction added
        // see if can be approved
        // add fees
        
        // if approved, update balance

        return new DepositResponse
        {
            TransactionStatus = transaction.Status,
        };
    }
}
