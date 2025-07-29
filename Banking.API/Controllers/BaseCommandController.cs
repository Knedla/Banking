using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.API.Controllers;

public abstract class BaseCommandController //: ControllerBase
{
    protected async Task/*<ActionResult*/<TOutput>/*>*/ ExecuteCommand<TInput, TOutput>(
        ICommandHandler<TInput, TOutput> command,
        TInput input,
        CancellationToken ct) 
        where TInput : BaseRequest
        where TOutput : BaseResponse, new()
    {
        var ctx = new CommandContext<TInput, TOutput>(input);
        await command.ExecuteAsync(ctx, ct);
        return /*Ok(*/ctx.Output/*)*/;
    }
}
