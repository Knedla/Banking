using Banking.Application.Commands.Common;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.Transfer
{
    public class TransferExecutionTransactionCommandHandler
        : IExecutionTransactionCommandHandler<TransferRequest, TransferResponse>
    {
        private readonly ITransferService _transferService;

        public TransferExecutionTransactionCommandHandler(ITransferService transferService)
        {
            _transferService = transferService;
        }

        public Task<bool> CanExecuteAsync(CommandContext<TransferRequest, TransferResponse> context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public async Task ExecuteAsync(CommandContext<TransferRequest, TransferResponse> context, CancellationToken cancellationToken)
        {
            var result = await _transferService.TransferAsync(context.Input);
            context.Output = result;
        }
    }
}
