namespace Banking.Application.Commands.Common
{
    public interface ICommandHandler<TInput, TOutput>
    {
        Task<bool> CanExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken cancellationToken);
        Task ExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken cancellationToken);
    }
}
