using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities.Transactions;
using Banking.Domain.Enumerations;
using Banking.Domain.Interfaces.Polices;
using Banking.Domain.Models;

namespace Banking.Application.Services;

public class TransactionApprovalService : ITransactionApprovalService
{
    private readonly ITransactionApprovalPolicy _transactionApprovalPolicy;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ITransactionService _transactionService;

    public TransactionApprovalService(
        ITransactionApprovalPolicy transactionApprovalPolicy,
        IDomainEventDispatcher domainEventDispatcher,
        ITransactionService transactionService)
    {
        _transactionApprovalPolicy = transactionApprovalPolicy;
        _domainEventDispatcher = domainEventDispatcher;
        _transactionService = transactionService;
    }

    public async Task<ApprovalDecision> ApproveWithRelatedTransactionsAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        // PROMENA STATUSA TREBA DA SE DESAVA NA SVIM TRANSAKCIJAMA I ONIM SUB I ORIGINALNOJ
        // trigger for every subtransactions... !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // ne mora svaka subtransakcija da se apruvuje, ali one sto nisu u krajnjim statusima bi trebalo da se pomere sa trenutnog na approve

        // if mony out - update AvailableBalance, update on transaction added / any status change 
        // za odlazece transakcije fijeva, treba da napravim ulazne transakcije u banku

        // TRANSFER PREBACI SA JEDNOG RACUNA NA DRUGI !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // if approved triger execute


        await _domainEventDispatcher.RaiseAsync(new TransactionWithRelatedTransactionsApprovedEvent(
                transaction.Id,
                transaction.TransactionInitializedById ?? Guid.Empty // InvolvedPartyId prop should be removed from IDomainEvent, eventually
            ));


        throw new NotImplementedException();


        // await _transactionService.CompleteAsync(transaction, currentUserId, cancellationToken); //////////////////////////////////////////////////////////////////////////////////////////////////////
        /*
        
        */

        // ne mogu da stavim da se okida event i uApproveAsync
        // onda ce svaki put kad odavde pozovem to da se okine event ... a treba mi ako su sve transakcije odobremen da moze da se izvrsi
    }

    public async Task<ApprovalDecision> ApproveAsync(Transaction transaction, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        var approvalDecision = await _transactionApprovalPolicy.EvaluateAsync(transaction, currentUserId, cancellationToken);

        if (approvalDecision.IsApproved)
            await _transactionService.ChangeStatusAsync(transaction, TransactionStatus.Approved, currentUserId, cancellationToken); // ovo ce da se trigeruje na svaku transakciju.... 
        else
        {
            var timestamp = DateTime.UtcNow;
            transaction.RequiresApproval = true;
            transaction.ApprovalStatus = ApprovalStatus.Pending;
            if (transaction.ApprovalRequirements == null)
                transaction.ApprovalRequirements = new List<TransactionApprovalRequirement>();

            var approvalRequirements = await _transactionApprovalPolicy.GetRequirements(transaction);
            foreach (var item in approvalRequirements)
                transaction.ApprovalRequirements.Add(
                    new TransactionApprovalRequirement() // TODO: add mapper
                    {
                        Id = Guid.NewGuid(),
                        TransactionId = transaction.Id,
                        RuleName = item.RuleName,
                        RequiredRoles = item.RequiredRoles,
                        ApprovalGroups = item.ApprovalGroups,
                        MinimumApprovals = item.MinimumApprovals,
                        Justification = item.Justification,
                        CreatedAt = timestamp
                    });
            await _transactionService.UpdateAsync(transaction, currentUserId, cancellationToken);
        }

        return approvalDecision;
    }
}
