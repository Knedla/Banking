using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.Transfer
{
    public class TransferExecutionCommand : IExecutionCommand<TransferRequest, TransferResponse>
    {
        //private readonly IAccountRepository _repo;
        //public TransferExecutionCommand(IAccountRepository repo) => _repo = repo;

        public Task<bool> CanExecuteAsync(CommandContext<TransferRequest, TransferResponse> ctx, CancellationToken ct)
            => Task.FromResult(true);

        public async Task ExecuteAsync(CommandContext<TransferRequest, TransferResponse> ctx, CancellationToken ct)
        {
            //var fromBalance = await _repo.WithdrawAsync(ctx.Input.FromAccountId, ctx.Input.Amount, ct);
            //var toBalance = await _repo.DepositAsync(ctx.Input.ToAccountId, ctx.Input.Amount, ct);
            //ctx.Output = new(ctx.Input.FromAccountId, ctx.Input.ToAccountId, fromBalance, toBalance);
        }
    }
}
