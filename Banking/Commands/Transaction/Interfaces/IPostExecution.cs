using Banking.Commands.Interfaces;

namespace Banking.Commands.Transaction.Interfaces
{
    public interface IPostExecution<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
