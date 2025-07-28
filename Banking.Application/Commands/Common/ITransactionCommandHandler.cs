namespace Banking.Application.Commands.Common;

// ideas:
// attribude
// [Ordering(Group : 1, Item : 1, WaitForPrewiousItemToFinish: false)]
// [Ordering(Group : 1, Item : 2, WaitForPrewiousItemToFinish: false)]
// [Ordering(Group : 2, Item : 1, WaitForPrewiousItemToFinish: true)]
// items from group 1 would be executed in parallel
// group 2 would wait for all items of group 1 to be executed
// that execution logic should be implemented in TransactionCommandHandler
//
// also add a parameter to determine whether the TransactionCommandHandler should wait for the transaction command result
// for example, the execution transaction is complete, but the post-execution transaction needs to do something, and the result of that is not needed
// because of that, the TransactionCommandHandler command does not have to wait for a response, but can continue with the execution
public interface ITransactionCommandHandler<TInput, TOutput> : ICommandHandler<TInput, TOutput> { }
