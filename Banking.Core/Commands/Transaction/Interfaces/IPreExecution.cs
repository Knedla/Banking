using Banking.Core.Commands.Interfaces;

namespace Banking.Core.Commands.Transaction.Interfaces
{
    public interface IPreExecution<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
