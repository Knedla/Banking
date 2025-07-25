using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.Withdrawal
{
    public class WithdrawalExecutionCommand : IExecutionCommand<WithdrawalRequest, WithdrawalResponse>
    {
        //private readonly IAccountRepository _repo;
        //public WithdrawalExecutionCommand(IAccountRepository repo) => _repo = repo;

        public Task<bool> CanExecuteAsync(CommandContext<WithdrawalRequest, WithdrawalResponse> ctx, CancellationToken ct)
            => Task.FromResult(true);

        public async Task ExecuteAsync(CommandContext<WithdrawalRequest, WithdrawalResponse> ctx, CancellationToken ct)
        {
            //var balance = await _repo.WithdrawAsync(ctx.Input.AccountId, ctx.Input.Amount, ct);
            //ctx.Output = new(ctx.Input.AccountId, balance);
        }
    }
}
