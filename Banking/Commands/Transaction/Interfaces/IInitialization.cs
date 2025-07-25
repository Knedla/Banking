using Banking.Commands.Interfaces;

namespace Banking.Commands.Transaction.Interfaces
{
    public interface IInitialization<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
