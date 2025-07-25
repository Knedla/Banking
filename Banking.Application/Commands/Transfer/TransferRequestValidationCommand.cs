using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;
using System.ComponentModel.DataAnnotations;

namespace Banking.Application.Commands.Transfer
{
    public class TransferRequestValidationCommand : IValidationCommand<TransferRequest, TransferResponse>
    {
        public Task<bool> CanExecuteAsync(CommandContext<TransferRequest, TransferResponse> ctx, CancellationToken ct)
            => Task.FromResult(true);

        public Task ExecuteAsync(CommandContext<TransferRequest, TransferResponse> ctx, CancellationToken ct)
        {
            if (ctx.Input.Amount <= 0)
                throw new ValidationException("Transfer amount must be positive");

            if (ctx.Input.FromAccountId == ctx.Input.ToAccountId)
                throw new ValidationException("Cannot transfer to the same account");

            return Task.CompletedTask;
        }
    }
}
