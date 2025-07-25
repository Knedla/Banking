namespace Banking.Application.Commands.Common
{
    public interface IExecutionCommand<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
