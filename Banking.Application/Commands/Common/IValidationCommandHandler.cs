namespace Banking.Application.Commands.Common
{
    public interface IValidationCommandHandler<TInput, TOutput> : ICommandHandler<TInput, TOutput> { }
}
