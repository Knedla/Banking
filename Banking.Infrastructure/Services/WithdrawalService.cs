using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;

namespace Banking.Infrastructure.Services;

public class WithdrawalService : IWithdrawalService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IFraudDetectionService _fraudDetectionService;
    private readonly ITransactionApprovalService _transactionApprovalService;
    private readonly ITransactionFeeService _transactionFeeService;
    private readonly IPolicyService<IWithdrawalPolicy> _policyService;

    public WithdrawalService(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        IFraudDetectionService fraudDetectionService,
        ITransactionApprovalService transactionApprovalService,
        IPolicyService<IWithdrawalPolicy> policyService)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _fraudDetectionService = fraudDetectionService;
        _transactionApprovalService = transactionApprovalService;
        _policyService = policyService;
    }

    public async Task<WithdrawalResponse> WithdrawAsync(WithdrawalRequest request)
    {
        if (request == null)
            throw new Exception($"Request is null.");

        if (request.AccountId == Guid.Empty) // TODO: or any alternative key
            throw new Exception($"AccountId is null.");

        var account = await _accountRepository.GetByIdAsync(request.AccountId);

        if (account == null)
            throw new Exception($"Cannot find account.");

        if (!account.Balances.Any(s => s.CurrencyCode == request.CurrencyCode)) // it should never happen
            throw new Exception($"Currency not suported.");

        var currencyAmount = new CurrencyAmount() { Amount = request.Amount, CurrencyCode = request.CurrencyCode };
        var timestamp = DateTime.UtcNow;
        var transaction = new Transaction()
        {
            Id = Guid.NewGuid(), // TODO: implement IdGenereator
            InvolvedPartyId = request.InvolvedPartyId,
            // RelatedToTransactionId
            // ReversalTransactionId
            Timestamp = timestamp,
            Type = TransactionType.Withdrawal,
            Status = TransactionStatus.Pending,
            Channel = request.TransactionChannel,
            AccountId = account.Id,
            // Description
            // CounterpartyAccountDetails

            InitCurrencyAmount = currencyAmount,
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

        await _transactionRepository.AddAsync(transaction); // trigger event transaction added if needed

        if (await _fraudDetectionService.IsSuspiciousTransactionAsync(transaction, CancellationToken.None)) // AML calculation
        {
            transaction.Status = TransactionStatus.Suspended;
            await _transactionRepository.UpdateAsync(transaction);

            var result = new WithdrawalResponse()
            {
                TransactionStatus = transaction.Status
            };
            result.AddError("ALM watching you!");
            return result;
        }

        await _transactionFeeService.AddFeesAsync(transaction, request.UserId, CancellationToken.None);

        var transactionPolicyResult = await _policyService.EvaluatePoliciesAsync(transaction, request.UserId, CancellationToken.None);
        if (transactionPolicyResult.Any(s => !s.IsSuccess))
        {
            transaction.Status = TransactionStatus.Voided;
            await _transactionRepository.UpdateAsync(transaction);

            var result = new WithdrawalResponse()
            {
                TransactionStatus = transaction.Status
            };
            foreach (var item in transactionPolicyResult.Where(s => s!.IsSuccess))
                result.AddError(item.ErrorMessage);
            return result;
        }

        await _transactionApprovalService.ApproveWithRelatedTransactionsAsync(transaction, request.UserId, CancellationToken.None);

        return new WithdrawalResponse { TransactionStatus = transaction.Status, };
    }
}
