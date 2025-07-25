namespace Banking.Application.Commands.Common
{
    public interface IValidationCommand<TInput, TOutput> : ICommand<TInput, TOutput> { }
}
