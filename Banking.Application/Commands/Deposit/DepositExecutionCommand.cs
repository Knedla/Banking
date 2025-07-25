using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.Deposit
{
    public class DepositExecutionCommand : IExecutionCommand<DepositRequest, DepositResponse>
    {
        //private readonly IAccountRepository _repo;
        //public DepositExecutionCommand(IAccountRepository repo) => _repo = repo;

        public Task<bool> CanExecuteAsync(CommandContext<DepositRequest, DepositResponse> ctx, CancellationToken ct)
            => Task.FromResult(true);

        public async Task ExecuteAsync(CommandContext<DepositRequest, DepositResponse> ctx, CancellationToken ct)
        {
            //var balance = await _repo.DepositAsync(ctx.Input.AccountId, ctx.Input.Amount, ct);
            //ctx.Output = new(ctx.Input.AccountId, balance);
        }
    }
}
