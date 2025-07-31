using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.ValueObjects;
using Newtonsoft.Json;

namespace Banking.Application.Services;

public class TransactionFeeService : ITransactionFeeService
{
    private static readonly Guid systemUserId = Guid.NewGuid(); // TODO: centralize getting systemUserId !!!

    private readonly ITransactionFeePolicy _transactionFeePolicy;
    private readonly IInsertFeeTransactionService _insertFeeTransactionService;

    public TransactionFeeService(
        ITransactionFeePolicy transactionFeePolicy,
        IInsertFeeTransactionService insertFeeTransactionService)
    {
        _transactionFeePolicy = transactionFeePolicy;
        _insertFeeTransactionService = insertFeeTransactionService;
    }

    public async Task AddFeesAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        var fees = await _transactionFeePolicy.EvaluateAsync(transaction, cancellationToken);

        if (fees == null && fees.Count == 0)
            return;

        if (transaction.RelatedTransactions == null)
            transaction.RelatedTransactions = new List<Transaction>();

        var timestamp = DateTime.UtcNow;
        foreach (var fee in fees)
        {
            var amount = fee.Type == FeeType.Flat ? fee.Amount : transaction.CalculatedCurrencyAmount.Amount * fee.Amount;
            var currencyAmount = new CurrencyAmount() { Amount = amount, CurrencyCode = fee.CurrencyCode }; // expecting fee to be same currency as transaction.CalculatedCurrencyAmount.CurrencyCode -> use ICurrencyExchangeService if needed

            var fromTransactionAccountDetails = (transaction.Type == TransactionType.Deposit) ? transaction.ToTransactionAccountDetails : transaction.FromTransactionAccountDetails; // TODO: resolve this for real

            var transactionFee = new Transaction()
            {
                Id = Guid.NewGuid(), // TODO: implement IdGenereator
                // TransactionInitializedById
                RelatedToTransactionId = transaction.Id,
                // ReversalTransactionId
                Timestamp = timestamp,
                Type = TransactionType.Fee,
                Status = TransactionStatus.Posted, // probably some minipipeline should be created for fees because they are not an independent transaction but part of the main one. this value is set for convenience
                Channel = TransactionChannel.System,
                Description = fee.Code,

                FromTransactionAccountDetails = fromTransactionAccountDetails,
                ToTransactionAccountDetails = new TransactionAccountDetails()
                {
                    AccountNumber = fee.AccountNumber,
                },
                CounterpartyAccountDetails = new CounterpartyAccountDetails()
                {
                    PaymentReference = JsonConvert.SerializeObject(fee)
                },

                FromCurrencyAmount = currencyAmount,
                // ExchangeRate
                CalculatedCurrencyAmount = currencyAmount,

                RequiresApproval = false,
                ApprovalStatus = ApprovalStatus.NotRequired,
                IsDeleted = false,
                CreatedAt = timestamp,
                CreatedByUserId = systemUserId,
                LastModifiedAt = timestamp,
                LastModifiedByUserId = systemUserId,

                // RelatedTransactions
                // ApprovalRequirements
                // Approvals
                // Batches
            };
            transaction.RelatedTransactions.Add(transactionFee);
            await _insertFeeTransactionService.AddAsync<TransactionResponse>(transactionFee, currentUserId, cancellationToken); // should I do something with result ?
        }
    }
}
