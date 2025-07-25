using Banking.Core.Commands.Interfaces;

namespace Banking.Core.Commands.Transaction.Interfaces
{
    public interface IValidation<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
