using Banking.Core.Commands.Interfaces;

namespace Banking.Core.Commands.Transaction.Interfaces
{
    public interface IInitialization<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
