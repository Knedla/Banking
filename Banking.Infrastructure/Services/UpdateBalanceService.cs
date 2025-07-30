using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities;
using Banking.Domain.Entities.Accounts;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;
using Banking.Domain.Repositories;
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

        if (transaction.Type == TransactionType.Withdrawal)
            await UpdateFromTransactionAccount(transaction);
        else if (transaction.Type == TransactionType.Deposit)
            await UpdateToFromTransactionAccount(transaction);
        else if (transaction.Type == TransactionType.Transfer || transaction.Type == TransactionType.Fee)
        {
            await UpdateFromTransactionAccount(transaction);

            if (transaction.ToTransactionAccountDetails == null)
                throw new Exception($"ToTransactionAccountDetails is null.");

            if (await transaction.ToTransactionAccountDetails.CheckIfTheAccountBelongsToTheSystem(_accountRepository))
                await UpdateToFromTransactionAccount(transaction);
        }
    }

    async Task UpdateFromTransactionAccount(Transaction transaction)
    {
        if (transaction.FromTransactionAccountDetails == null)
            throw new Exception($"FromTransactionAccountDetails is null.");

        var account = await transaction.FromTransactionAccountDetails.TryResolveAccount(_accountRepository);

        if (account == null)
            throw new Exception($"Cannot find FromTransactionAccount account.");

        UpdateBalance(transaction, account, TransactionHoldingType.Outgoing);
        await _accountRepository.UpdateAsync(account);
    }

    async Task UpdateToFromTransactionAccount(Transaction transaction)
    {
        if (transaction.ToTransactionAccountDetails == null)
            throw new Exception($"ToTransactionAccountDetails is null.");

        var account = await transaction.ToTransactionAccountDetails.TryResolveAccount(_accountRepository);

        if (account == null)
            throw new Exception($"Cannot find ToTransactionAccount account.");

        UpdateBalance(transaction, account, TransactionHoldingType.Incomming);
        await _accountRepository.UpdateAsync(account);
    }

    void UpdateBalance(Transaction transaction, Account account, TransactionHoldingType transactionHoldingType)
    {
        var holding = GetTransactionHolding(account, transaction);

        if (transactionHoldingType == TransactionHoldingType.Incomming)
        {
            if (_transactionStateValidator.GetCommitToBalanceStatuses().Contains(transaction.Status))
                CommitToBalanceStatuses(account, holding, transactionHoldingType);
        }
        else
        {
            if (_transactionStateValidator.GetDoNotApplyToAccountBalanceStatuses().Contains(transaction.Status))
                RemoveFromAvailableBalance(account, holding, transactionHoldingType);
            else if (_transactionStateValidator.GetApplyToActualBalanceStatuses().Contains(transaction.Status))
                ApplyToAvailableBalance(account, holding, transactionHoldingType);
            else if (_transactionStateValidator.GetRollbackFromActualBalanceStatuses().Contains(transaction.Status))
                RemoveFromAvailableBalance(account, holding, transactionHoldingType);
            else if (_transactionStateValidator.GetCommitToBalanceStatuses().Contains(transaction.Status))
                CommitToBalanceStatuses(account, holding, transactionHoldingType);
        }
    }

    void ApplyToAvailableBalance(Account account, TransactionHolding holding, TransactionHoldingType transactionHoldingType)
    {
        if (holding.IsAppliedToBalance || holding.IsAppliedToAvailableBalance)
            return;

        var accountBalance = GetAccountBalance(account, holding.Transaction.CalculatedCurrencyAmount.CurrencyCode);

        holding.IsAppliedToAvailableBalance = true;
        if (transactionHoldingType == TransactionHoldingType.Incomming)
            accountBalance.AvailableBalance += holding.Transaction.CalculatedCurrencyAmount.Amount;
        else if (transactionHoldingType == TransactionHoldingType.Outgoing)
            accountBalance.AvailableBalance -= holding.Transaction.CalculatedCurrencyAmount.Amount;
    }

    void RemoveFromAvailableBalance(Account account, TransactionHolding holding, TransactionHoldingType transactionHoldingType)
    {
        if (holding.IsAppliedToBalance || !holding.IsAppliedToAvailableBalance)
            return;

        var accountBalance = GetAccountBalance(account, holding.Transaction.CalculatedCurrencyAmount.CurrencyCode);

        holding.IsAppliedToAvailableBalance = false;
        if (transactionHoldingType == TransactionHoldingType.Incomming)
            accountBalance.AvailableBalance -= holding.Transaction.CalculatedCurrencyAmount.Amount;
        else if (transactionHoldingType == TransactionHoldingType.Outgoing)
            accountBalance.AvailableBalance += holding.Transaction.CalculatedCurrencyAmount.Amount;
    }

    void CommitToBalanceStatuses(Account account, TransactionHolding holding, TransactionHoldingType transactionHoldingType) // should be removed from account.Holdings after Balance is updated ?
    {
        if (holding.IsAppliedToBalance)
            return;

        var accountBalance = GetAccountBalance(account, holding.Transaction.CalculatedCurrencyAmount.CurrencyCode);

        if (!holding.IsAppliedToAvailableBalance)
            ApplyToAvailableBalance(account, holding, transactionHoldingType);

        holding.IsAppliedToBalance = true;
        if (transactionHoldingType == TransactionHoldingType.Incomming)
            accountBalance.Balance += holding.Transaction.CalculatedCurrencyAmount.Amount;
        else if (transactionHoldingType == TransactionHoldingType.Outgoing)
            accountBalance.Balance -= holding.Transaction.CalculatedCurrencyAmount.Amount;
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
            holding = new TransactionHolding()
            {
                TransactionId = transaction.Id,
                IsAppliedToAvailableBalance = false,
                IsAppliedToBalance = false,
                Transaction = transaction
            };
            account.Holdings.Add(holding);
        }
        return holding;
    }
}
