using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Plicies;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;
using Newtonsoft.Json;

namespace Banking.Application.Services;

public class TransactionFeeService : ITransactionFeeService
{
    private static readonly Guid systemId = Guid.NewGuid(); // TODO: centralize this !!!

    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionFeePolicy _transactionFeePolicy;
    private readonly IAccountRepository _accountRepository;

    public TransactionFeeService(
        ITransactionRepository transactionRepository,
        ITransactionFeePolicy transactionFeePolicy,
        IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _transactionFeePolicy = transactionFeePolicy;
        _accountRepository = accountRepository;
    }

    public async Task AddFeesAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        var fees = await _transactionFeePolicy.EvaluateAsync(transaction, cancellationToken);

        if (fees == null && fees.Count == 0)
            return;

        var account = await _accountRepository.GetByIdAsync(transaction.AccountId);

        if (transaction.RelatedTransactions == null)
            transaction.RelatedTransactions = new List<Transaction>();

        var timestamp = DateTime.UtcNow;
        foreach (var fee in fees)
        {
            var amount = fee.Type == FeeType.Flat ? fee.Amount : transaction.CalculatedCurrencyAmount.Amount * fee.Amount;
            var currencyAmount = new CurrencyAmount() { Amount = amount, CurrencyCode = fee.CurrencyCode }; // expecting fee to be same currency as transaction.CalculatedCurrencyAmount.CurrencyCode -> use ICurrencyExchangeService if needed

            var transactionFee = new Transaction()
            {
                Id = Guid.NewGuid(), // TODO: implement IdGenereator
                RelatedToTransactionId = transaction.Id,
                // ReversalTransactionId
                Timestamp = timestamp,
                Type = TransactionType.Fee,
                Status = TransactionStatus.Pending,
                Channel = TransactionChannel.System,
                AccountId = transaction.AccountId,
                Description = fee.Code,
                RecipientDetails = new RecipientDetails() { AccountNumber = fee.AccountNumber, PaymentReference = JsonConvert.SerializeObject(fee) },

                InitCurrencyAmount = currencyAmount,
                // ExchangeRate
                CalculatedCurrencyAmount = currencyAmount,

                RequiresApproval = false,
                ApprovalStatus = ApprovalStatus.NotRequired,
                IsDeleted = false,
                CreatedAt = timestamp,
                CreatedByUserId = systemId,
                LastModifiedAt = timestamp,
                LastModifiedByUserId = systemId,

                // RelatedTransactions
                // ApprovalRequirements
                // Approvals
                // Batches
            };
            transaction.RelatedTransactions.Add(transactionFee);
            await _transactionRepository.AddAsync(transaction);
        }
    }
}
