using Banking.Application.Commands.Common;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.Withdrawal;

public class WithdrawalExecutionTransactionCommandHandler
    : IExecutionTransactionCommandHandler<WithdrawalRequest, WithdrawalResponse>
{
    private readonly IWithdrawalService _withdrawalService;

    public WithdrawalExecutionTransactionCommandHandler(IWithdrawalService withdrawalService)
    {
        _withdrawalService = withdrawalService;
    }

    public Task<bool> CanExecuteAsync(CommandContext<WithdrawalRequest, WithdrawalResponse> context, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public async Task ExecuteAsync(CommandContext<WithdrawalRequest, WithdrawalResponse> context, CancellationToken cancellationToken)
    {
        var result = await _withdrawalService.WithdrawAsync(context.Input);
        context.Output = result;
    }
}
