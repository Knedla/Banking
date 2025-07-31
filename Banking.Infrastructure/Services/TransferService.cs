using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;
using Banking.Infrastructure.Extensaions;

namespace Banking.Infrastructure.Services;

public class TransferService : ITransferService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyExchangeService _currencyExchangeService;
    private readonly IInsertTransactionService<ITransferPolicy> _insertTransactionService;
    private readonly IAuthorizationPolicyService<IInitializeTransactionAuthorizationPolicy> _authorizationPolicyService;

    public TransferService(
        IAccountRepository accountRepository,
        ICurrencyExchangeService currencyExchangeService,
        IInsertTransactionService<ITransferPolicy> insertTransactionService,
        IAuthorizationPolicyService<IInitializeTransactionAuthorizationPolicy> authorizationPolicyService)
    {
        _accountRepository = accountRepository;
        _currencyExchangeService = currencyExchangeService;
        _insertTransactionService = insertTransactionService;
        _authorizationPolicyService = authorizationPolicyService;
    }

    public async Task<TransferResponse> TransferAsync(TransferRequest request)
    {
        if (request == null)
            throw new Exception($"Request is null.");

        var fromAccount = await request.FromTransactionAccountDetails.TryResolveAccount(_accountRepository); // should always be from this bank, to belong to the initiator of the transaction ?
        var toAccount = await request.ToTransactionAccountDetails.TryResolveAccount(_accountRepository);

        if (fromAccount == null)
            throw new Exception($"Cannot resolve FromAccount.");
        else if (!fromAccount.Balances.Any(s => s.CurrencyCode == request.FromCurrencyCode))
            throw new Exception($"From currency {request.FromCurrencyCode} not suported.");

        if (request.TransactionInitializedById != null)
        {
            var authorizationPolicyResult = await _authorizationPolicyService.EvaluatePoliciesAsync(request.TransactionInitializedById.Value, fromAccount.Id);
            if (authorizationPolicyResult.Any(s => !s.IsSuccess))
            {
                var response = new TransferResponse();
                foreach (var item in authorizationPolicyResult.Where(s => s!.IsSuccess))
                    response.AddError(item.ErrorMessage);
                return response;
            }
        }

        ExchangeRate exchangeRate = null;

        if (!string.IsNullOrEmpty(request.ToCurrencyCode)) // maybe I was overthinking this part with different currencies
        {
            if (toAccount != null && !toAccount.Balances.Any(s => s.CurrencyCode == request.ToCurrencyCode))
                throw new Exception($"To currency {request.ToCurrencyCode} not suported.");

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
            Type = TransactionType.Transfer,
            Status = TransactionStatus.Pending, // this value is set for convenience
            Channel = request.TransactionChannel,
            // Description
            FromTransactionAccountDetails = new TransactionAccountDetails()
            {
                AccountId = request.FromTransactionAccountDetails.AccountId,
                AccountNumber = request.FromTransactionAccountDetails.AccountNumber,
                Iban = request.FromTransactionAccountDetails.Iban,
                SwiftCode = request.FromTransactionAccountDetails.SwiftCode,
                Account = fromAccount
            },
            ToTransactionAccountDetails = new TransactionAccountDetails()
            {
                AccountId = request.ToTransactionAccountDetails.AccountId,
                AccountNumber = request.ToTransactionAccountDetails.AccountNumber,
                Iban = request.ToTransactionAccountDetails.Iban,
                SwiftCode = request.ToTransactionAccountDetails.SwiftCode,
                Account = toAccount
            },
            CounterpartyAccountDetails = new CounterpartyAccountDetails() // TODO: add auto mapper
            {
                Name = request.CounterpartyAccountDetails.Name,
                BankName = request.CounterpartyAccountDetails.BankName,
                BankAddress = request.CounterpartyAccountDetails.BankAddress,
                PaymentReference = request.CounterpartyAccountDetails.PaymentReference,
                ExternalReferenceCode = request.CounterpartyAccountDetails.ExternalReferenceCode,
                Priority = request.CounterpartyAccountDetails.Priority,
            },

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

        return await _insertTransactionService.AddAsync<TransferResponse>(transaction, request.UserId, CancellationToken.None);
    }
}
