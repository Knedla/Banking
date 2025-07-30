using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities;
using Banking.Domain.Entities.Accounts;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.StateMachine;
using Banking.Domain.Repositories;

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

        var account = await _accountRepository.GetByIdAsync(transaction.AccountId);

        if (account == null)
            throw new Exception($"Cannot find account.");


        // if mony out - update AvailableBalance, update on transaction added / any status change 
        // za odlazece transakcije fijeva, treba da napravim ulazne transakcije u banku
        // TRANSFER PREBACI SA JEDNOG RACUNA NA DRUGI !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!



        var holding = GetTransactionHolding(account, transaction);

        if (holding.Type == TransactionHoldingType.Incomming)
        {
            if (_transactionStateValidator.GetCommitToBalanceStatuses().Contains(transaction.Status))
                CommitToBalanceStatuses(account, holding);
        }
        else
        {
            if (_transactionStateValidator.GetDoNotApplyToAccountBalanceStatuses().Contains(transaction.Status))
                RemoveFromAvailableBalance(account, holding);
            else if (_transactionStateValidator.GetApplyToActualBalanceStatuses().Contains(transaction.Status))
                ApplyToAvailableBalance(account, holding);
            else if (_transactionStateValidator.GetRollbackFromActualBalanceStatuses().Contains(transaction.Status))
                RemoveFromAvailableBalance(account, holding);
            else if (_transactionStateValidator.GetCommitToBalanceStatuses().Contains(transaction.Status))
                CommitToBalanceStatuses(account, holding);
        }
        await _accountRepository.UpdateAsync(account);
    }

    void ApplyToAvailableBalance(Account account, TransactionHolding holding)
    {
        if (holding.IsAppliedToBalance || holding.IsAppliedToAvailableBalance)
            return;

        var accountBalance = GetAccountBalance(account, holding.Transaction.CalculatedCurrencyAmount.CurrencyCode);

        holding.IsAppliedToAvailableBalance = true;
        if (holding.Type == TransactionHoldingType.Incomming)
            accountBalance.AvailableBalance += holding.Transaction.CalculatedCurrencyAmount.Amount;
        else if (holding.Type == TransactionHoldingType.Outgoing)
            accountBalance.AvailableBalance -= holding.Transaction.CalculatedCurrencyAmount.Amount;
    }

    void RemoveFromAvailableBalance(Account account, TransactionHolding holding)
    {
        if (holding.IsAppliedToBalance || !holding.IsAppliedToAvailableBalance)
            return;

        var accountBalance = GetAccountBalance(account, holding.Transaction.CalculatedCurrencyAmount.CurrencyCode);

        holding.IsAppliedToAvailableBalance = false;
        if (holding.Type == TransactionHoldingType.Incomming)
            accountBalance.AvailableBalance -= holding.Transaction.CalculatedCurrencyAmount.Amount;
        else if (holding.Type == TransactionHoldingType.Outgoing)
            accountBalance.AvailableBalance += holding.Transaction.CalculatedCurrencyAmount.Amount;
    }

    void CommitToBalanceStatuses(Account account, TransactionHolding holding) // should be removed from account.Holdings after Balance is updated ?
    {
        if (holding.IsAppliedToBalance)
            return;

        var accountBalance = GetAccountBalance(account, holding.Transaction.CalculatedCurrencyAmount.CurrencyCode);

        if (!holding.IsAppliedToAvailableBalance)
            ApplyToAvailableBalance(account, holding);

        holding.IsAppliedToBalance = true;
        if (holding.Type == TransactionHoldingType.Incomming)
            accountBalance.Balance += holding.Transaction.CalculatedCurrencyAmount.Amount;
        else if (holding.Type == TransactionHoldingType.Outgoing)
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
                Type =
                    transaction.Type == TransactionType.Withdrawal ||
                    transaction.Type == TransactionType.Transfer ||
                    transaction.Type == TransactionType.Fee // mock calculation
                        ? TransactionHoldingType.Outgoing
                        : TransactionHoldingType.Incomming,
                IsAppliedToAvailableBalance = false,
                IsAppliedToBalance = false,
                Transaction = transaction
            };
            account.Holdings.Add(holding);
        }
        return holding;
    }
}
