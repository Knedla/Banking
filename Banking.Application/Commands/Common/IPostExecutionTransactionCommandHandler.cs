namespace Banking.Application.Commands.Common
{
    public interface IPostExecutionTransactionCommandHandler<TInput, TOutput> : ITransactionCommandHandler<TInput, TOutput> { }
}
