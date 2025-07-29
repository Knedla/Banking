using Banking.Application.Interfaces.Services;
using Banking.Domain.Entities.Transactions;
using Microsoft.Extensions.Logging;

namespace Banking.Infrastructure.Services
{
    public class FraudDetectionService : IFraudDetectionService // mock
    {
        private const decimal amount  = 10000;

        private readonly ILogger<FraudDetectionService> _logger;

        public FraudDetectionService(ILogger<FraudDetectionService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> IsSuspiciousTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            // Example logic (in real life, call DB, ML model, or third-party API)
            _logger.LogInformation("Checking fraud for withdrawal: {Amount}", transaction.CalculatedCurrencyAmount.Amount);
            await Task.Delay(50, cancellationToken); // simulate latency
            return transaction.CalculatedCurrencyAmount.Amount > amount; // flag large transaction as suspicious
        }
    }
}
