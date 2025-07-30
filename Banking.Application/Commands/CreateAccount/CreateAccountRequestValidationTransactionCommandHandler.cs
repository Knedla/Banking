using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using Banking.Domain.Enumerations;

namespace Banking.Application.Commands.CreateAccount;

public class CreateAccountRequestValidationTransactionCommandHandler : IValidationTransactionCommandHandler<CreateAccountRequestRequest, CreateAccountRequestResponse>
{
    public Task<bool> CanExecuteAsync(CommandContext<CreateAccountRequestRequest, CreateAccountRequestResponse> ctx, CancellationToken ct)
        => Task.FromResult(true);

    public Task ExecuteAsync(CommandContext<CreateAccountRequestRequest, CreateAccountRequestResponse> ctx, CancellationToken ct)
    {
        if (ctx.Input.InvolvedPartyId == Guid.Empty)
            ctx.Output.AddError("InvolvedPartyId is required");

        if (ctx.Input.AccountType is (AccountType.Loan or AccountType.Credit)) // mock
            ctx.Output.AddError("Invalid account type");

        return Task.CompletedTask;
    }
}
