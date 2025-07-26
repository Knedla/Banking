namespace Banking.Application.Commands.Common
{
    public interface IExecutionCommandHandler<TInput, TOutput> : ICommandHandler<TInput, TOutput> { }
}
