namespace Banking.Application.Commands.Common
{
    public interface IInitializationCommand<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
