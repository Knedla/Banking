using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using System.ComponentModel.DataAnnotations;

namespace Banking.Application.Commands.AccountBalance;

public class AccountBalanceRequestValidationTransactionCommandHandler : IValidationTransactionCommandHandler<AccountBalanceRequest, AccountBalanceResponse>
{
    public Task<bool> CanExecuteAsync(CommandContext<AccountBalanceRequest, AccountBalanceResponse> ctx, CancellationToken ct)
        => Task.FromResult(true);

    public Task ExecuteAsync(CommandContext<AccountBalanceRequest, AccountBalanceResponse> ctx, CancellationToken ct)
    {
        if (ctx.Input.AccountId == null)
            throw new ValidationException("AccountId is required");

        return Task.CompletedTask;
    }
}
