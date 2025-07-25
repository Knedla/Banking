namespace Banking.Core.Commands.Interfaces
{
    public interface ICommand<TInput, TOutput>
    {
        Task<bool> CanExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken cancellationToken);
        Task ExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken cancellationToken);
    }
}
