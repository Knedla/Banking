using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.AccountBalance
{
    public class AccountBalanceExecutionCommand : IExecutionCommand<AccountBalanceRequest, AccountBalanceResponse>
    {
        //private readonly IAccountRepository _repo;
        //public AccountBalanceExecutionCommand(IAccountRepository repo) => _repo = repo;

        public Task<bool> CanExecuteAsync(CommandContext<AccountBalanceRequest, AccountBalanceResponse> ctx, CancellationToken ct)
            => Task.FromResult(true);

        public async Task ExecuteAsync(CommandContext<AccountBalanceRequest, AccountBalanceResponse> ctx, CancellationToken ct)
        {
            //var balance = await _repo.GetBalanceAsync(ctx.Input.AccountId, ct);
            //ctx.Output = new(ctx.Input.AccountId, balance);
        }
    }
}
