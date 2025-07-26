using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using System.ComponentModel.DataAnnotations;

namespace Banking.Application.Commands.Deposit
{
    public class DepositRequestValidationTransactionCommandHandler : IValidationTransactionCommandHandler<DepositRequest, DepositResponse>
    {
        public Task<bool> CanExecuteAsync(CommandContext<DepositRequest, DepositResponse> ctx, CancellationToken ct)
            => Task.FromResult(true);

        public Task ExecuteAsync(CommandContext<DepositRequest, DepositResponse> ctx, CancellationToken ct)
        {
            if (ctx.Input.Amount <= 0)
                throw new ValidationException("Deposit amount must be positive");

            return Task.CompletedTask;
        }
    }
}
