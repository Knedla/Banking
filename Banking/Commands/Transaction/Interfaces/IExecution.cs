using Banking.Commands.Interfaces;

namespace Banking.Commands.Transaction.Interfaces
{
    public interface IExecution<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
