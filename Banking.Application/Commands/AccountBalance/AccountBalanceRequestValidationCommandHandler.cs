using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using System.ComponentModel.DataAnnotations;

namespace Banking.Application.Commands.AccountBalance;

public class AccountBalanceRequestValidationTransactionCommandHandler : IValidationTransactionCommandHandler<AccountBalanceRequest, AccountBalanceResponse>
{
    public Task<bool> CanExecuteAsync(CommandContext<AccountBalanceRequest, AccountBalanceResponse> ctx, CancellationToken ct)
        => Task.FromResult(true);

    public Task ExecuteAsync(CommandContext<AccountBalanceRequest, AccountBalanceResponse> ctx, CancellationToken ct) // TODO: could be better implemented, with validation rules or something ...
    {
        if (ctx.Input.AccountId == Guid.Empty)
            throw new ValidationException("AccountId is required");

        if (ctx.Input.RequestingInvolvedPartyId == Guid.Empty)
            throw new ValidationException("RequestingInvolvedPartyId is required");

        return Task.CompletedTask;
    }
}
