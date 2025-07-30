using Banking.Application.Commands.Common;
using Banking.Application.Events;
using Banking.Application.Interfaces;
using Banking.Application.Interfaces.Services;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Responses;

namespace Banking.Application.Commands.Transfer;

public class TransferExecutionTransactionCommandHandler
    : IExecutionTransactionCommandHandler<TransferRequest, TransferResponse>
{
    private readonly ITransferService _transferService;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public TransferExecutionTransactionCommandHandler(ITransferService transferService, IDomainEventDispatcher domainEventDispatcher)
    {
        _transferService = transferService;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public Task<bool> CanExecuteAsync(CommandContext<TransferRequest, TransferResponse> context, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public async Task ExecuteAsync(CommandContext<TransferRequest, TransferResponse> context, CancellationToken cancellationToken)
    {
        var result = await _transferService.TransferAsync(context.Input);
        context.Output = result;

        var evt = new TransactionExecutedEvent( // TODO: centralize events creation 
            result.TransactionId,
            result.SourceAccountId,
            result.DestinationAccountNumber,
            result.InvolvedPartyId,
            result.Amount,
            result.Currency,
            result.IsSuccess,
            DateTime.UtcNow
        );

        await _domainEventDispatcher.RaiseAsync(evt);
    }
}
