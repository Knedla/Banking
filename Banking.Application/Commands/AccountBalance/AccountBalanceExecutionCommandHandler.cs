using Banking.Application.Commands.Common;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.AccountBalance
{
    public class AccountBalanceExecutionTransactionCommandHandler
        : IExecutionTransactionCommandHandler<AccountBalanceRequest, AccountBalanceResponse>
    {
        private readonly IGetBalanceService _getBalanceService;

        public AccountBalanceExecutionTransactionCommandHandler(IGetBalanceService getBalanceService)
        {
            _getBalanceService = getBalanceService;
        }

        public Task<bool> CanExecuteAsync(CommandContext<AccountBalanceRequest, AccountBalanceResponse> context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public async Task ExecuteAsync(CommandContext<AccountBalanceRequest, AccountBalanceResponse> context, CancellationToken cancellationToken)
        {
            var result = await _getBalanceService.GetBalanceAsync(context.Input);
            context.Output = result;
        }
    }
}
