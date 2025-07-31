using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities;
using Banking.Domain.Entities.Accounts;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;
using Banking.Infrastructure.Extensaions;

namespace Banking.Infrastructure.Services;

public class UpdateBalanceService : IUpdateBalanceService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionStateValidator _transactionStateValidator;

    public UpdateBalanceService(
        IAccountRepository accountRepository,
        ITransactionStateValidator transactionStateValidator)
    {
        _accountRepository = accountRepository;
        _transactionStateValidator = transactionStateValidator;
    }

    public async Task UpdateBalanceAsync(Transaction transaction)
    {
        if (transaction == null)
            throw new Exception($"Request is null.");

        if (transaction.Type == TransactionType.Withdrawal || 
            transaction.Type == TransactionType.Transfer || 
            transaction.Type == TransactionType.Fee)
            await UpdateFromTransactionAccount(transaction);
        
        if (transaction.Type == TransactionType.Deposit || 
            transaction.Type == TransactionType.Transfer || 
            transaction.Type == TransactionType.Fee)
            await UpdateToFromTransactionAccount(transaction);
    }

    async Task UpdateFromTransactionAccount(Transaction transaction)
    {
        if (!await transaction.FromTransactionAccountDetails.CheckIfTheAccountBelongsToTheSystem(_accountRepository))
            return;

        if (transaction.FromTransactionAccountDetails == null)
            throw new Exception($"FromTransactionAccountDetails is null.");

        var account = await transaction.FromTransactionAccountDetails.TryResolveAccount(_accountRepository);

        if (account == null)
            throw new Exception($"Cannot find FromTransactionAccount account.");

        var holding = GetTransactionHolding(account, transaction);

        if (_transactionStateValidator.GetDoNotApplyToAccountBalanceStatuses().Contains(transaction.Status) ||
            _transactionStateValidator.GetRollbackFromActualBalanceStatuses().Contains(transaction.Status))
            RemoveFromAvailableBalance(transaction, account, holding.OutgoingApplication, TransactionHoldingType.Outgoing);
        else if (_transactionStateValidator.GetApplyToActualBalanceStatuses().Contains(transaction.Status))
            ApplyToAvailableBalance(transaction, account, holding.OutgoingApplication, TransactionHoldingType.Outgoing);
        else if (_transactionStateValidator.GetCommitToBalanceStatuses().Contains(transaction.Status))
            CommitToBalanceStatuses(transaction, account, holding.OutgoingApplication, TransactionHoldingType.Outgoing);
        
        await _accountRepository.UpdateAsync(account);
    }

    async Task UpdateToFromTransactionAccount(Transaction transaction)
    {
        if (!await transaction.ToTransactionAccountDetails.CheckIfTheAccountBelongsToTheSystem(_accountRepository))
            return;

        if (transaction.ToTransactionAccountDetails == null)
            throw new Exception($"ToTransactionAccountDetails is null.");

        var account = await transaction.ToTransactionAccountDetails.TryResolveAccount(_accountRepository);

        if (account == null)
            throw new Exception($"Cannot find ToTransactionAccount account.");

        var holding = GetTransactionHolding(account, transaction);

        if (_transactionStateValidator.GetCommitToBalanceStatuses().Contains(transaction.Status))
            CommitToBalanceStatuses(transaction, account, holding.IncommingApplication, TransactionHoldingType.Incomming);
        
        await _accountRepository.UpdateAsync(account);
    }

    void ApplyToAvailableBalance(Transaction transaction, Account account, TransactionHoldingApplication transactionHoldingApplication, TransactionHoldingType transactionHoldingType)
    {
        if (transactionHoldingApplication.IsAppliedToBalance || 
            transactionHoldingApplication.IsAppliedToAvailableBalance)
            return;

        transactionHoldingApplication.IsAppliedToAvailableBalance = true;
        if (transactionHoldingType == TransactionHoldingType.Incomming)
            AddToAvailableBalance(account, transaction.CalculatedCurrencyAmount);
        else
            RemoveFromAvailableBalance(account, transaction.FromCurrencyAmount);
    }

    void RemoveFromAvailableBalance(Transaction transaction, Account account, TransactionHoldingApplication transactionHoldingApplication, TransactionHoldingType transactionHoldingType)
    {
        if (transactionHoldingApplication.IsAppliedToBalance || 
            !transactionHoldingApplication.IsAppliedToAvailableBalance)
            return;

        transactionHoldingApplication.IsAppliedToAvailableBalance = false;
        if (transactionHoldingType == TransactionHoldingType.Incomming)
            RemoveFromAvailableBalance(account, transaction.CalculatedCurrencyAmount);
        else
            AddToAvailableBalance(account, transaction.FromCurrencyAmount);
    }

    // should be removed from account.Holdings after Balance is updated or set holding as Completed?
    void CommitToBalanceStatuses(Transaction transaction, Account account, TransactionHoldingApplication transactionHoldingApplication, TransactionHoldingType transactionHoldingType)
    {
        if (transactionHoldingApplication.IsAppliedToBalance)
            return;

        if (!transactionHoldingApplication.IsAppliedToAvailableBalance)
            ApplyToAvailableBalance(transaction, account, transactionHoldingApplication, transactionHoldingType);

        transactionHoldingApplication.IsAppliedToBalance = true;
        if (transactionHoldingType == TransactionHoldingType.Incomming)
            AddToBalance(account, transaction.CalculatedCurrencyAmount);
        else
            RemoveFromBalance(account, transaction.FromCurrencyAmount);
    }

    void AddToAvailableBalance(Account account, CurrencyAmount currencyAmount)
    {
        var accountBalance = GetAccountBalance(account, currencyAmount.CurrencyCode);
        accountBalance.AvailableBalance += currencyAmount.Amount;
    }

    void RemoveFromAvailableBalance(Account account, CurrencyAmount currencyAmount)
    {
        var accountBalance = GetAccountBalance(account, currencyAmount.CurrencyCode);
        accountBalance.AvailableBalance -= currencyAmount.Amount;
    }

    void AddToBalance(Account account, CurrencyAmount currencyAmount)
    {
        var accountBalance = GetAccountBalance(account, currencyAmount.CurrencyCode);
        accountBalance.Balance += currencyAmount.Amount;
    }

    void RemoveFromBalance(Account account, CurrencyAmount currencyAmount)
    {
        var accountBalance = GetAccountBalance(account, currencyAmount.CurrencyCode);
        accountBalance.Balance -= currencyAmount.Amount;
    }

    AccountBalance GetAccountBalance(Account account, string currencyCode)
    {
        var accountBalance = account.Balances.FirstOrDefault(s => s.CurrencyCode == currencyCode);

        if (accountBalance == null)
            throw new Exception($"Cannot find account balance {currencyCode}.");

        return accountBalance;
    }

    TransactionHolding GetTransactionHolding(Account account, Transaction transaction)
    {
        if (account.Holdings == null)
            account.Holdings = new List<TransactionHolding>();

        var holding = account.Holdings.FirstOrDefault(s => s.TransactionId == transaction.Id);

        if (holding == null)
        {
            holding = new TransactionHolding() // TODO: implement a builder to instanciate IncommingApplication and OutgoingApplication depending on the TransactionType
            {
                TransactionId = transaction.Id,
                IncommingApplication = new TransactionHoldingApplication()
                {
                    IsAppliedToAvailableBalance = false,
                    IsAppliedToBalance = false,
                },
                OutgoingApplication = new TransactionHoldingApplication()
                {
                    IsAppliedToAvailableBalance = false,
                    IsAppliedToBalance = false,
                },
                Transaction = transaction
            };
            account.Holdings.Add(holding);
        }
        return holding;
    }
}
