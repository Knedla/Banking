using Banking.Application.Commands.Common;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.Deposit
{
    public class DepositExecutionCommandHandler
        : IExecutionCommandHandler<DepositRequest, DepositResponse>
    {
        private readonly IDepositService _depositService;

        public DepositExecutionCommandHandler(IDepositService depositService)
        {
            _depositService = depositService;
        }

        public Task<bool> CanExecuteAsync(CommandContext<DepositRequest, DepositResponse> context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public async Task ExecuteAsync(CommandContext<DepositRequest, DepositResponse> context, CancellationToken cancellationToken)
        {
            var result = await _depositService.DepositAsync(context.Input);
            context.Output = result;
        }
    }
}
