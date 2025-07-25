using Banking.Application.Commands.Common;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.CreateAccount
{
    public class CreateAccountExecutionCommand : IExecutionCommand<CreateAccountRequest, CreateAccountResponse>
    {
        //private readonly IAccountRepository _repo;
        //public CreateAccountExecutionCommand(IAccountRepository repo) => _repo = repo;

        public Task<bool> CanExecuteAsync(CommandContext<CreateAccountRequest, CreateAccountResponse> ctx, CancellationToken ct)
            => Task.FromResult(true);

        public async Task ExecuteAsync(CommandContext<CreateAccountRequest, CreateAccountResponse> ctx, CancellationToken ct)
        {
            //var accountId = Guid.NewGuid().ToString();
            //await _repo.CreateAsync(accountId, ctx.Input.CustomerId, ctx.Input.AccountType, ct);
            //ctx.Output = new(accountId, 0);
        }
    }
}
