namespace Banking.Application.Commands.Common
{
    public interface IPostExecutionCommand<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
