namespace Banking.Application.Commands.Common
{
    public interface IInitializationCommandHandler<TInput, TOutput> : ICommandHandler<TInput, TOutput> { }
}
