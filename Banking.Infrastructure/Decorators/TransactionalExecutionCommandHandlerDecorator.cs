using Banking.Application.Commands.Common;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Decorators;

public class TransactionalExecutionCommandHandlerDecorator<TInput, TOutput> : IExecutionTransactionCommandHandler<TInput, TOutput>
{
    private readonly IExecutionTransactionCommandHandler<TInput, TOutput> _inner;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionalExecutionCommandHandlerDecorator(
        IExecutionTransactionCommandHandler<TInput, TOutput> inner,
        IUnitOfWork unitOfWork)
    {
        _inner = inner;
        _unitOfWork = unitOfWork;
    }

    public Task<bool> CanExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken cancellationToken)
    {
        return _inner.CanExecuteAsync(context, cancellationToken);
    }

    public async Task ExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _inner.ExecuteAsync(context, cancellationToken);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
