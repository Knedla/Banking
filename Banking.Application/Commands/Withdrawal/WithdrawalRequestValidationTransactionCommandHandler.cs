using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using System.ComponentModel.DataAnnotations;

namespace Banking.Application.Commands.Withdrawal
{
    public class WithdrawalRequestValidationTransactionCommandHandler : IValidationTransactionCommandHandler<WithdrawalRequest, WithdrawalResponse>
    {
        public Task<bool> CanExecuteAsync(CommandContext<WithdrawalRequest, WithdrawalResponse> ctx, CancellationToken ct)
            => Task.FromResult(true);

        public Task ExecuteAsync(CommandContext<WithdrawalRequest, WithdrawalResponse> ctx, CancellationToken ct)
        {
            if (ctx.Input.Amount <= 0)
                throw new ValidationException("Withdrawal amount must be positive");

            return Task.CompletedTask;
        }
    }
}
