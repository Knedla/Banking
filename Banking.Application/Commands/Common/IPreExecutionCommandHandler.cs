namespace Banking.Application.Commands.Common
{
    public interface IPreExecutionCommandHandler<TInput, TOutput> : ICommandHandler<TInput, TOutput> { }
}
