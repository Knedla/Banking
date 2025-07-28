using Banking.Application.Commands.Common;

namespace Banking.Application.Interfaces.Factories;

public interface ITransactionCommandHandlerFactory
{
    ICommandHandler<TInput, TOutput> Create<TInput, TOutput>();
}
