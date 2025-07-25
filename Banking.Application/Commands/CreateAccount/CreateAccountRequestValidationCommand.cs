using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using System.ComponentModel.DataAnnotations;

namespace Banking.Application.Commands.CreateAccount
{
    public class CreateAccountRequestValidationCommand : IValidationCommand<CreateAccountRequest, CreateAccountResponse>
    {
        public Task<bool> CanExecuteAsync(CommandContext<CreateAccountRequest, CreateAccountResponse> ctx, CancellationToken ct)
            => Task.FromResult(true);

        public Task ExecuteAsync(CommandContext<CreateAccountRequest, CreateAccountResponse> ctx, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(ctx.Input.CustomerId))
                throw new ValidationException("CustomerId is required");

            if (ctx.Input.AccountType is not ("Checking" or "Savings"))
                throw new ValidationException("Invalid account type");

            return Task.CompletedTask;
        }
    }
}
