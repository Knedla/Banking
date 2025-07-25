namespace Banking.Application.Commands.Common
{
    public interface IPreExecutionCommand<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
