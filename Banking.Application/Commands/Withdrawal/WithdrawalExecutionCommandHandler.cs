using Banking.Application.Commands.Common;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.Withdraw
{
    public class WithdrawExecutionCommandHandler
        : IExecutionCommandHandler<WithdrawalRequest, WithdrawalResponse>
    {
        private readonly IWithdrawService _withdrawService;

        public WithdrawExecutionCommandHandler(IWithdrawService withdrawService)
        {
            _withdrawService = withdrawService;
        }

        public Task<bool> CanExecuteAsync(CommandContext<WithdrawalRequest, WithdrawalResponse> context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public async Task ExecuteAsync(CommandContext<WithdrawalRequest, WithdrawalResponse> context, CancellationToken cancellationToken)
        {
            var result = await _withdrawService.WithdrawAsync(context.Input);
            context.Output = result;
        }
    }
}
