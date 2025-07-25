using Banking.Commands.Interfaces;

namespace Banking.Commands.Transaction.Interfaces
{
    public interface IPreExecution<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
