using Banking.Core.Commands.Interfaces;

namespace Banking.Core.Commands.Transaction.Interfaces
{
    public interface IExecution<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
