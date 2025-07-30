using Banking.Domain.Enumerations;

namespace Banking.Domain.Interfaces.StateMachine;

public interface ITransactionStateValidator : IStateValidator<TransactionStatus>
{
    HashSet<TransactionStatus> GetDoNotApplyToAccountBalanceStatuses();
    HashSet<TransactionStatus> GetApplyToActualBalanceStatuses();
    HashSet<TransactionStatus> GetRollbackFromActualBalanceStatuses();
    HashSet<TransactionStatus> GetCommitToBalanceStatuses();
    HashSet<TransactionStatus> GetEndTransactionStatuses();
}
