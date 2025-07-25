using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using System.ComponentModel.DataAnnotations;

namespace Banking.Application.Commands.AccountBalance
{
    public class AccountBalanceRequestValidationCommand : IValidationCommand<AccountBalanceRequest, AccountBalanceResponse>
    {
        public Task<bool> CanExecuteAsync(CommandContext<AccountBalanceRequest, AccountBalanceResponse> ctx, CancellationToken ct)
            => Task.FromResult(true);

        public Task ExecuteAsync(CommandContext<AccountBalanceRequest, AccountBalanceResponse> ctx, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(ctx.Input.AccountId))
                throw new ValidationException("AccountId is required");

            return Task.CompletedTask;
        }
    }
}
