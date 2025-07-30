using Banking.Domain.Entities.Transactions;
using Banking.Domain.ValueObjects;

namespace Banking.Infrastructure.Extensaions
{
    public static class TransactionExtensaions
    {
        public static Dictionary<string, decimal> SumByFromCurrencyCode(this Transaction transaction)
        {
            var allAmounts = new List<CurrencyAmount>
            {
                transaction.FromCurrencyAmount
            };

            // Add related transactions' CurrencyAmounts
            if (transaction.RelatedTransactions != null)
                foreach (var related in transaction.RelatedTransactions)
                    allAmounts.Add(related.FromCurrencyAmount);

            // Group by CurrencyCode and sum the Amounts
            return allAmounts
                .GroupBy(x => x.CurrencyCode)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => x.Amount)
                );
        }
    }
}
