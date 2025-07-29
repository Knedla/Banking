using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.Common;

public interface ICommandHandler<TInput, TOutput>
    where TInput : BaseRequest
    where TOutput : BaseResponse, new()
{
    Task<bool> CanExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken cancellationToken);
    Task ExecuteAsync(CommandContext<TInput, TOutput> context, CancellationToken cancellationToken);
}
