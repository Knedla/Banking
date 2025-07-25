using Banking.Commands.Interfaces;

namespace Banking.Commands.Transaction.Interfaces
{
    public interface IValidation<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
