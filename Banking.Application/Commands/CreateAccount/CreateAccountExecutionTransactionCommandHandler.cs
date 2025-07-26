using Banking.Application.Commands.Common;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.CreateAccount
{
    public class CreateAccountExecutionTransactionCommandHandler
        : IExecutionTransactionCommandHandler<CreateAccountRequest, CreateAccountResponse>
    {
        private readonly ICreateAccountService _createAccountService;

        public CreateAccountExecutionTransactionCommandHandler(ICreateAccountService createAccountService)
        {
            _createAccountService = createAccountService;
        }

        public Task<bool> CanExecuteAsync(CommandContext<CreateAccountRequest, CreateAccountResponse> context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public async Task ExecuteAsync(CommandContext<CreateAccountRequest, CreateAccountResponse> context, CancellationToken cancellationToken)
        {
            var result = await _createAccountService.CreateAccountAsync(context.Input);
            context.Output = result;
        }
    }
}
