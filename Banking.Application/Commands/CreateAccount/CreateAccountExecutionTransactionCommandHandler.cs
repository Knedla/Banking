using Banking.Application.Commands.Common;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.CreateAccount;

public class CreateAccountExecutionTransactionCommandHandler
    : IExecutionTransactionCommandHandler<CreateAccountRequestRequest, CreateAccountRequestResponse>
{
    private readonly ICreateAccountRequestService _createAccountService;

    public CreateAccountExecutionTransactionCommandHandler(ICreateAccountRequestService createAccountService)
    {
        _createAccountService = createAccountService;
    }

    public Task<bool> CanExecuteAsync(CommandContext<CreateAccountRequestRequest, CreateAccountRequestResponse> context, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public async Task ExecuteAsync(CommandContext<CreateAccountRequestRequest, CreateAccountRequestResponse> context, CancellationToken cancellationToken)
    {
        var result = await _createAccountService.CreateWorkItemAsync(context.Input);
        context.Output = result;
    }
}
