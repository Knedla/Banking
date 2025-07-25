using Banking.Core.Commands.Interfaces;

namespace Banking.Core.Commands.Transaction.Interfaces
{
    public interface IPostExecution<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
