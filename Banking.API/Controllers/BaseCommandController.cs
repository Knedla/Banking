using Banking.Application.Commands.Common;

namespace Banking.API.Controllers;

public abstract class BaseCommandController //: ControllerBase
{
    protected async Task/*<ActionResult*/<TOutput>/*>*/ ExecuteCommand<TInput, TOutput>(
        ICommandHandler<TInput, TOutput> command,
        TInput input,
        CancellationToken ct)
    {
        var ctx = new CommandContext<TInput, TOutput>(input);
        await command.ExecuteAsync(ctx, ct);
        return /*Ok(*/ctx.Output/*)*/;
    }
}
