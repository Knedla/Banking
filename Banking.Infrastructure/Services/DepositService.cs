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
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyExchangeService _currencyExchangeService;
    private readonly IFraudDetectionService _fraudDetectionService;
    private readonly ITransactionApprovalService _transactionApprovalService;
    private readonly ITransactionFeeService _transactionFeeService;

    public DepositService(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        ICurrencyExchangeService currencyExchangeService,
        IFraudDetectionService fraudDetectionService,
        ITransactionApprovalService transactionApprovalService)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _currencyExchangeService = currencyExchangeService;
        _fraudDetectionService = fraudDetectionService;
        _transactionApprovalService = transactionApprovalService;
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
        var transaction = new Transaction()
        {
            Id = Guid.NewGuid(), // TODO: implement IdGenereator
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
        
        await _transactionRepository.AddAsync(transaction); // trigger event transaction added if needed

        var isSuspiciousTransaction = await _fraudDetectionService.IsSuspiciousTransactionAsync(transaction, CancellationToken.None); // AML calculation
        if (isSuspiciousTransaction)
        {
            transaction.Status = TransactionStatus.Suspended;
            await _transactionRepository.UpdateAsync(transaction);

            var result = new DepositResponse();
            result.AddError("ALM watching you!");
            return result;
        }

        await _transactionFeeService.AddFeesAsync(transaction, request.UserId, CancellationToken.None);

        // trigger policies min balance izracunaj dal ima sa svim fijevima 

        await _transactionApprovalService.ApproveAsync(transaction, request.UserId, CancellationToken.None);

        return new DepositResponse
        {
            TransactionStatus = transaction.Status,
        };
    }
}
