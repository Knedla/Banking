namespace Banking.Application.Commands.Common
{
    public interface IPostExecutionCommandHandler<TInput, TOutput> : ICommandHandler<TInput, TOutput> { }
}
